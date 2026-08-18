namespace PortfolioApi.Services.DTOs;

public class WeatherResponseDto
{
    public string City { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double TemperatureCelsius { get; set; }
    public double FeelsLikeCelsius { get; set; }
    public double WindSpeed { get; set; }
}
