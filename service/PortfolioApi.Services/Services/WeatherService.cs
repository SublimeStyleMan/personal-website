using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace PortfolioApi.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<WeatherResponseDto?> GetCurrentWeatherAsync(string city)
    {
        var apiKey = _configuration["OpenWeather:ApiKey"];
        var baseUrl = _configuration["OpenWeather:BaseUrl"];

        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("OpenWeather configuration is missing.");
        }

        var url = $"{baseUrl}/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (!root.TryGetProperty("main", out var main) ||
            !root.TryGetProperty("weather", out var weather) ||
            !root.TryGetProperty("wind", out var wind))
        {
            return null;
        }

        var description = weather[0].GetProperty("description").GetString() ?? "Unknown";

        return new WeatherResponseDto
        {
            City = root.GetProperty("name").GetString() ?? city,
            Description = description,
            TemperatureCelsius = main.GetProperty("temp").GetDouble(),
            FeelsLikeCelsius = main.GetProperty("feels_like").GetDouble(),
            WindSpeed = wind.GetProperty("speed").GetDouble()
        };
    }
}
