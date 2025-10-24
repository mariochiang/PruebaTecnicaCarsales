namespace Carsales.Bff.Api.Models;

public sealed class ApiInfo
{
    public int Count { get; set; }
    public int Pages { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
}

public sealed class ApiEpisode
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Air_Date { get; set; } = "";
    public string Episode { get; set; } = "";
    public string Url { get; set; } = "";
    public string Created { get; set; } = "";
}

public sealed class ApiPagedEpisodes
{
    public ApiInfo Info { get; set; } = new();
    public List<ApiEpisode> Results { get; set; } = new();
}
