using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Carsales.Bff.Api.Configuration;
using Carsales.Bff.Api.Models;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Carsales.Bff.Api.Services;

public sealed class RickAndMortyService : IRickAndMortyService
{
    private readonly HttpClient _http;
    private readonly RickAndMortyOptions _opts;

    
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retry;

    public RickAndMortyService(HttpClient http, IOptions<RickAndMortyOptions> opts)
    {
        _http = http;
        _opts = opts.Value;

        var delays = Backoff.DecorrelatedJitterBackoffV2(
            TimeSpan.FromMilliseconds(250), _opts.RetryCount);

        _retry = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r =>
                (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(delays);
    }

    public async Task<PaginatedResponse<EpisodeDto>> GetEpisodesAsync(int page, string? name, CancellationToken ct)
    {
        var url = $"episode?page={page}";
        if (!string.IsNullOrWhiteSpace(name))
            url += $"&name={Uri.EscapeDataString(name)}";

        var response = await _retry.ExecuteAsync(() => _http.GetAsync(url, ct));
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<ApiPagedEpisodes>(cancellationToken: ct)
                   ?? new ApiPagedEpisodes();

        const int pageSize = 20;

        var items = data.Results.Select(e => new EpisodeDto
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Episode,
            AirDate = e.Air_Date
        }).ToList();

        return new PaginatedResponse<EpisodeDto>
        {
            Total = data.Info.Count,
            Pages = data.Info.Pages,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<EpisodeDto?> GetEpisodeByIdAsync(int id, CancellationToken ct)
    {
        var response = await _retry.ExecuteAsync(() => _http.GetAsync($"episode/{id}", ct));

        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();

        var e = await response.Content.ReadFromJsonAsync<ApiEpisode>(cancellationToken: ct);
        if (e == null) return null;

        return new EpisodeDto
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Episode,
            AirDate = e.Air_Date
        };
    }
}
