using AzureFunctionsREST.API.Models;
using FluentValidation;

namespace AzureFunctionsREST.API.Validators
{
    public class StationRequestValidator : AbstractValidator<StationRequest>
    {
        public StationRequestValidator()
        {
            RuleFor(station => station.Name).NotEmpty().WithMessage("Station must have a name");
            RuleFor(station => station.Location).NotEmpty().WithMessage("Station must have a location");
        }
    }
}