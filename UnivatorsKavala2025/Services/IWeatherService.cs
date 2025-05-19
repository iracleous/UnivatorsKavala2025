using Microsoft.AspNetCore.Mvc;
using UnivatorsKavala2025.Models;

namespace UnivatorsKavala2025.Services;

public interface IWeatherService
{
    Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync();
    Task<ActionResult<WeatherForecast>> GetWeatherForecastByIdAsync(int id);
    Task<ActionResult<WeatherForecast>> AddWeatherForecastAsync(WeatherForecast weatherForecast);
    Task<IActionResult> UpdateWeatherForecastAsync(int id, WeatherForecast weatherForecast);
    Task<ActionResult> DeleteWeatherForecastAsync(int id);
}
