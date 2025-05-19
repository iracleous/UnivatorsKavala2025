using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Data;
using UnivatorsKavala2025.Models;

namespace UnivatorsKavala2025.Services;


public class WeatherService : IWeatherService
{
    private readonly WeatherDbContext _context;

    public WeatherService(WeatherDbContext context)
    {
        _context = context;
    }

 
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync()
    {
        return await _context.WeatherForecasts.ToListAsync();
    }

 
    public async Task<ActionResult<WeatherForecast>> GetWeatherForecastByIdAsync(int id)
    {
        var weatherForecast = await _context.WeatherForecasts.FindAsync(id);

        if (weatherForecast == null)
        {
            return new NotFoundResult();
        }

        return weatherForecast;
    }

 
    public async Task<IActionResult> UpdateWeatherForecastAsync(int id, WeatherForecast weatherForecast)
    {
        if (id != weatherForecast.Id)
        {
            return new NotFoundResult();
        }

        _context.Entry(weatherForecast).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WeatherForecastExists(id))
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NotFoundResult();
    }

 
    public async Task<ActionResult<WeatherForecast>> AddWeatherForecastAsync(WeatherForecast weatherForecast)
    {
        _context.WeatherForecasts.Add(weatherForecast);
        await _context.SaveChangesAsync();

        //  return new CreatedAtActionResult("GetWeatherForecast",null, null, weatherForecast);
        return weatherForecast;
    }

 
    public async Task<ActionResult> DeleteWeatherForecastAsync(int id)
    {
        var weatherForecast = await _context.WeatherForecasts.FindAsync(id);
        if (weatherForecast == null)
        {
            return  new NotFoundResult();
        }

        _context.WeatherForecasts.Remove(weatherForecast);
        await _context.SaveChangesAsync();

        return new NotFoundResult();
    }

    private bool WeatherForecastExists(int id)
    {
        return _context.WeatherForecasts.Any(e => e.Id == id);
    }
 
}

