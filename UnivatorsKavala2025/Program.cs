using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Data;
using UnivatorsKavala2025.Models;
using UnivatorsKavala2025.Services;
using UnivatorsKavala2025.Validators;

var builder = WebApplication.CreateBuilder(args);


// 1. Add CORS Services to the container
builder.Services.AddCors(options =>
{
    // Define a named CORS policy (e.g., "AllowSpecificOrigin")
    options.AddPolicy(name: "AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:54454"  // Your React/Angular/Vue app domain
                                ) // Another allowed domain
                  .AllowAnyHeader() // Allow all headers from these origins
                  .AllowAnyMethod(); // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
        });

    // You can define multiple policies, or a "catch-all" policy (use with caution)
    options.AddPolicy(name: "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Not recommended for production due to security risks
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWeatherService, WeatherService>();

// Register the validator with DI
builder.Services.AddScoped<IValidator<WeatherForecast>, WeatherForecastValidator>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. Enable CORS Middleware in the Configure method (or before MapControllers)
// Apply the named policy globally, or selectively via attributes.
app.UseCors("AllowSpecificOrigin"); // Apply the policy defined above by its name




// --- Automatic Migration Logic ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<WeatherDbContext>();
        context.Database.Migrate();
        // You can also add seed data here if needed
        // await SeedData.Initialize(services); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        // Consider re-throwing the exception or taking other action
    }
}
// --- End Automatic Migration Logic ---



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
