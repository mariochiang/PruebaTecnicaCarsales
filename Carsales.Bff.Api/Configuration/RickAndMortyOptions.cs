namespace Carsales.Bff.Api.Configuration;

public class RickAndMortyOptions
{
    public const string SectionName = "RickAndMorty";
    public string BaseUrl { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 15;
    public int RetryCount { get; set; } = 2;
}
