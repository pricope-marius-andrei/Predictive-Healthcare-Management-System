using Application.UseCases.Commands.Base;
using Domain.Common;
using FluentValidation;

namespace Application.UseCases.Commands.Patient
{
    public class CreatePatientCommandValidator : BaseUserCommandValidator<CreatePatientCommand, Result<Guid>>
    {
        public CreatePatientCommandValidator() : base()
        {
            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
        }
    }
}
