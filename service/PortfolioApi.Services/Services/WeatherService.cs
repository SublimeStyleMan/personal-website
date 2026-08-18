using System.Text.Json;
using Microsoft.Extensions.Options;
using PortfolioApi.Services.DTOs;
using PortfolioApi.Services.Interfaces;
using PortfolioApi.Services.Options;

namespace PortfolioApi.Services.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    public WeatherService(HttpClient httpClient, IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<WeatherResponseDto?> GetCurrentWeatherAsync(string city)
    {
        var apiKey = _options.ApiKey;
        var baseUrl = _options.BaseUrl;

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
