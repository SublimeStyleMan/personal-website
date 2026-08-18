namespace PortfolioApi.Services;

public interface IWeatherService
{
    Task<WeatherResponseDto?> GetCurrentWeatherAsync(string city);
}
