using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Commands
{
    public class BasePatientCommandValidator : BaseUserCommandValidator<UpdatePatientCommand, Result<Patient>>
    {
        public BasePatientCommandValidator() : base()
        {
            RuleFor(command => command.Address)
                    .NotEmpty().WithMessage("Address is required.")
                    .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");

            RuleFor(command => command.Gender)
                    .NotEmpty().WithMessage("Gender is required.")
                    .Must(BeAValidGender).WithMessage("Invalid gender.");

            RuleFor(command => command.Height)
                    .GreaterThanOrEqualTo(0).WithMessage("Height must be a non-negative value.");

            RuleFor(command => command.Weight)
                    .GreaterThanOrEqualTo(0).WithMessage("Weight must be a non-negative value.");

            RuleFor(command => command.DateOfBirth)
                    .NotEmpty().WithMessage("Date of birth is required.")
                    .Must(BeAValidDateOfBirth).WithMessage("Invalid date of birth.");

            RuleFor(command => command.DateOfRegistration)
                    .NotEmpty().WithMessage("Date of registration is required.")
                    .Must(BeAValidDateOfRegistration).WithMessage("Invalid date of registration.");
        }

        private static bool BeAValidGender(string? gender)
        {
            return gender == "Male" || gender == "Female";
        }

        private static bool BeAValidDateOfBirth(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Now;
        }

        private static bool BeAValidDateOfRegistration(DateTime dateOfRegistration)
        {
            return dateOfRegistration <= DateTime.Now;
        }
    }
}
