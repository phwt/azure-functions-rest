using AzureFunctionsREST.API.Models;
using FluentValidation;

namespace AzureFunctionsREST.API.Validators
{
    public class ReporterRequestValidator : AbstractValidator<ReporterRequest>
    {
        public ReporterRequestValidator()
        {
            RuleFor(reporter => reporter.Firstname).NotEmpty().WithMessage("Reporter must have a firstname");
            RuleFor(reporter => reporter.Lastname).NotEmpty().WithMessage("Reporter must have a lastname");
            RuleFor(reporter => reporter.Age).GreaterThan(0).WithMessage("Reporter age must be greater than zero");
        }
    }
}