using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Services.Interfaces;

namespace PortfolioApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    //<summary>
    /// Gets the current weather for a specified city.
    /// </summary>
    /// <param name="city">The city for which to get weather information.</param>
    /// <returns>The current weather for the specified city.</returns>
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("City is required.");
        }

        var weather = await _weatherService.GetCurrentWeatherAsync(city);

        if (weather == null)
        {
            return NotFound($"No weather data found for city: {city}");
        }

        return Ok(weather);
    }

    /// <summary>
    /// Gets the current weather for a specified location (latitude and longitude).
    /// </summary>
    /// <param name="lat">The latitude of the location.</param>
    /// <param name="lon">The longitude of the location.</param>
    /// <returns>The current weather for the specified location.</returns>
    [HttpGet("current-by-location")]
    public async Task<IActionResult> GetCurrentWeatherByLocation([FromQuery] string lat, [FromQuery] string lon)
    {
        if (string.IsNullOrWhiteSpace(lat) || string.IsNullOrWhiteSpace(lon))
        {
            return BadRequest("Latitude and longitude are required.");
        }

        var weather = await _weatherService.GetCurrentWeatherByLocationAsync(lat, lon);

        if (weather == null)
        {
            return NotFound($"No weather data found for the specified location.");
        }

        return Ok(weather);
    }
}

