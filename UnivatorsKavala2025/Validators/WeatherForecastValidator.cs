using FluentValidation;
using UnivatorsKavala2025.Models;

namespace UnivatorsKavala2025.Validators;

// Validator class using FluentValidation
public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
    public WeatherForecastValidator()
    {
        // Rule for Date: Should not be in the future
        RuleFor(forecast => forecast.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date cannot be in the future.");

        // Rule for TemperatureC: Should be within a reasonable range
        RuleFor(forecast => forecast.TemperatureC)
            .InclusiveBetween(-100, 100)
            .WithMessage("Temperature must be between -100 and 100°C.");

        // Rule for Summary: Should not be empty and have a maximum length
        RuleFor(forecast => forecast.Summary)
            .NotEmpty()
            .WithMessage("Summary is required.")
            .MaximumLength(200)
            .WithMessage("Summary cannot exceed 200 characters.");
        //Example of a more complex validation
        RuleFor(forecast => forecast.Summary)
            .Must(BeAValidSummary)
            .WithMessage("Summary must contain the word 'Sunny', 'Cloudy', or 'Rainy'.");
    }

    private bool BeAValidSummary(string? summary)
    {
        if (string.IsNullOrEmpty(summary)) return false;
        return summary.Contains("Sunny", StringComparison.OrdinalIgnoreCase) ||
               summary.Contains("Cloudy", StringComparison.OrdinalIgnoreCase) ||
               summary.Contains("Rainy", StringComparison.OrdinalIgnoreCase);
    }
}
