using FluentValidation;

namespace Application.UseCases.Commands
{
	public class UpdateDoctorCommandValidator : BaseDoctorCommandValidator<BaseDoctorCommand>
	{
		public UpdateDoctorCommandValidator()
		{

			RuleFor(command => command.DoctorId)
				.NotEmpty().WithMessage("Doctor ID is required.");

			AddDoctorRules();
		}
	}
}