using PortfolioApi.Services.DTOs;

namespace PortfolioApi.Services.Interfaces;

public interface IWeatherService
{
    Task<WeatherResponseDto?> GetCurrentWeatherAsync(string city);
}
