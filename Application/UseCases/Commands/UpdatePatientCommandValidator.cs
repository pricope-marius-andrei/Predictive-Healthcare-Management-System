using FluentValidation;

namespace Application.UseCases.Commands
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(command => command.PatientId)
                .NotEmpty().WithMessage("Patient ID is required.");

            RuleFor(command => command.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");

            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(command => command.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

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

        private static bool BeAValidGender(string gender)
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
