using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Data;
using UnivatorsKavala2025.Models;

namespace UnivatorsKavala2025.Services;


public class WeatherService : IWeatherService
{
    private readonly WeatherDbContext _context;
    private readonly ILogger<WeatherService> _logger;
    private readonly IValidator<WeatherForecast> _validator; // Inject the validator

    public WeatherService(WeatherDbContext context, ILogger<WeatherService> logger, IValidator<WeatherForecast> validator)
    {
        _context = context;
        _logger = logger;
        _validator = validator;
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

 
    public async Task<ActionResult<WeatherForecast>> AddWeatherForecastAsync(WeatherForecast forecast)
    {

        // 1. Perform the validation
        var validationResult = _validator.Validate(forecast);

        // 2. Check if the validation was successful
        if (!validationResult.IsValid)
        {
            _context.WeatherForecasts.Add(forecast);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Forecast is valid: {Date}, {TemperatureC}, {Summary}", forecast.Date, forecast.TemperatureC, forecast.Summary);

            //  return new CreatedAtActionResult("GetWeatherForecast",null, null, forecast);
            return forecast;
        }

        // 3. If validation failed, log the errors
        foreach (var error in validationResult.Errors)
        {
            _logger.LogError("Validation error: {PropertyName} - {ErrorMessage}", error.PropertyName, error.ErrorMessage);
        }
        // 4. Return a BadRequest with the validation errors    
        return new BadRequestObjectResult(validationResult.Errors);
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

