using System.Text.Json;
using Microsoft.Extensions.Options;
using PortfolioApi.Services.DTOs;
using PortfolioApi.Services.Interfaces;
using PortfolioApi.Services.Options;

namespace PortfolioApi.Services;

/// <summary>
/// Implements <see cref="IWeatherService"/> to fetch weather information from the OpenWeather API.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to call the OpenWeather API.</param>
    /// <param name="options">The configured OpenWeather options from the application settings.</param>
    public WeatherService(HttpClient httpClient, IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <summary>
    /// Retrieves the current weather for the specified city as defined by <see cref="IWeatherService"/>.
    /// </summary>
    /// <param name="city">The city name to look up.</param>
    /// <returns>The weather information for the city, or <see langword="null"/> if it cannot be retrieved.</returns>
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

    /// <summary>
    /// Retrieves the current weather using latitude and longitude coordinates.
    /// This method complements the contract defined in <see cref="IWeatherService"/>.
    /// </summary>
    /// <param name="lat">The latitude.</param>
    /// <param name="lon">The longitude.</param>
    /// <returns>The weather information for the given coordinates, or <see langword="null"/> if it cannot be retrieved.</returns>
    public async Task<WeatherResponseDto?> GetCurrentWeatherByLocationAsync(string lat, string lon)
    {
        var apiKey = _options.ApiKey;
        var baseUrl = _options.BaseUrl;

        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("OpenWeather configuration is missing.");
        }

        var url = $"{baseUrl}/weather?lat={Uri.EscapeDataString(lat)}&lon={Uri.EscapeDataString(lon)}&appid={apiKey}&units=metric";

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
            City = root.GetProperty("name").GetString() ?? "Unknown Location",
            Description = description,
            TemperatureCelsius = main.GetProperty("temp").GetDouble(),
            FeelsLikeCelsius = main.GetProperty("feels_like").GetDouble(),
            WindSpeed = wind.GetProperty("speed").GetDouble()
        };
    }
}
