using Application.UseCases.Commands.Base;
using FluentValidation;

namespace Application.UseCases.Commands.Patient
{
    public class UpdatePatientCommandValidator : BasePatientCommandValidator
    {
        public UpdatePatientCommandValidator() : base()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Patient ID is required.");
        }
    }
}
