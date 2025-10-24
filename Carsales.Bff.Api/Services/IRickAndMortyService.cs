using Carsales.Bff.Api.Models;

namespace Carsales.Bff.Api.Services;

public interface IRickAndMortyService
{
    Task<PaginatedResponse<EpisodeDto>> GetEpisodesAsync(int page, string? name, CancellationToken ct);
    Task<EpisodeDto?> GetEpisodeByIdAsync(int id, CancellationToken ct);
}
