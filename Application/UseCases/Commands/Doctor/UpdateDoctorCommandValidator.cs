using Application.UseCases.Commands.Base;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Commands.Doctor
{
    public class UpdateDoctorCommandValidator : BaseUserCommandValidator<UpdateDoctorCommand, Result<Domain.Entities.Doctor>>
    {
        public UpdateDoctorCommandValidator() : base()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Doctor ID is required.");
        }
    }
}
