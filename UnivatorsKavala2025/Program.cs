using Microsoft.EntityFrameworkCore;
using UnivatorsKavala2025.Data;
using UnivatorsKavala2025.Services;

var builder = WebApplication.CreateBuilder(args);


// 1. Add CORS Services to the container
builder.Services.AddCors(options =>
{
    // Define a named CORS policy (e.g., "AllowSpecificOrigin")
    options.AddPolicy(name: "AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5454"  // Your React/Angular/Vue app domain
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





app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
