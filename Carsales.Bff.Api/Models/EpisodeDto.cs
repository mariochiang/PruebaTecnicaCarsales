namespace Carsales.Bff.Api.Models;

public sealed class EpisodeDto
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Code { get; init; } = "";
    public string AirDate { get; init; } = "";
}
