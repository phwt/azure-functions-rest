using System;
using AzureFunctionsREST.API.Models;
using FluentValidation;

namespace AzureFunctionsREST.API.Validators
{
    public class WeatherForecastRequestValidator : AbstractValidator<WeatherForecastRequest>
    {
        public WeatherForecastRequestValidator()
        {
            RuleFor(forecast => forecast.Date).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Forecast date must be in the future");
            RuleFor(forecast => forecast.TemperatureC).InclusiveBetween(-100, 100)
                                                      .WithMessage("The specified temperature is exceed lowest/highest temperature ever recorded. Go somewhere else instead of logging the forecast.");
            RuleFor(forecast => forecast.Summary).NotEmpty().WithMessage("Forecast summary must be specified");
            RuleFor(forecast => forecast.Reporter).NotEmpty();
        }
    }
}