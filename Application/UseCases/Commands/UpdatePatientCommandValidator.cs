using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Commands
{
    public class UpdatePatientCommandValidator : BasePatientCommandValidator
    {
        public UpdatePatientCommandValidator() : base()
        {
            RuleFor(command => command.PatientId)
                .NotEmpty().WithMessage("Patient ID is required.");
        }
    }
}
