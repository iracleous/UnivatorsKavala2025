using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Models;

namespace UnivatorsKavala2025.Data;

public class WeatherDbContext:DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
    {
    }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
}
