using FluentValidation;

namespace Application.UseCases.Commands.BaseCommands
{
	public class BaseUserCommandValidator<T, U> : AbstractValidator<T> where T : BaseCommand<U>
	{
		public BaseUserCommandValidator()
		{
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
				.Length(10).WithMessage("Phone number must be 10 characters long.");
		}
	}
}