using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Data;
using UnivatorsKavala2025.Models;
using UnivatorsKavala2025.Services;

namespace UnivatorsKavala2025.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _service;

    public WeatherController(IWeatherService service)
    {
        _service = service;
    }

    // GET: api/WeatherForecasts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecasts()
    {
        return await _service.GetWeatherForecastsAsync();
    }

    // GET: api/WeatherForecasts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(int id)
    {
        return await _service.GetWeatherForecastByIdAsync(id);
    }

    // PUT: api/WeatherForecasts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWeatherForecast(int id, WeatherForecast weatherForecast)
    {
         return await _service.UpdateWeatherForecastAsync(id, weatherForecast);
    }

    // POST: api/WeatherForecasts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<WeatherForecast>> PostWeatherForecast(WeatherForecast weatherForecast)
    {
         return  await _service.AddWeatherForecastAsync(weatherForecast);
    }

    // DELETE: api/WeatherForecasts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeatherForecast(int id)
    {
        return await _service.DeleteWeatherForecastAsync(id);
    }
}
