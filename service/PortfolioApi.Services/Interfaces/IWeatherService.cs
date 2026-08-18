using PortfolioApi.Services.DTOs;

namespace PortfolioApi.Services.Interfaces;

public interface IWeatherService
{
    //<summary>
    /// Gets the current weather for a specified city.
    /// </summary>
    /// <param name="city">The city for which to get weather information.</param>
    /// <returns>The current weather for the specified city.</returns>  
    Task<WeatherResponseDto?> GetCurrentWeatherAsync(string city);

    /// <summary>
    /// Gets the current weather for a specified location (latitude and longitude).
    /// </summary>   
    /// <param name="lat">The latitude of the location.</param>
    /// <param name="lon">The longitude of the location.</param>
    /// <returns>The current weather for the specified location.</returns>   
    Task<WeatherResponseDto?> GetCurrentWeatherByLocationAsync(string lat, string lon);
}
