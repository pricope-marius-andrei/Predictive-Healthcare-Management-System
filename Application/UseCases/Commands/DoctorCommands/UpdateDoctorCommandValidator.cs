using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Commands.DoctorCommands
{
	public class UpdateDoctorCommandValidator : BaseUserCommandValidator<UpdateDoctorCommand, Result<Doctor>>
	{
		public UpdateDoctorCommandValidator() : base()
		{
			RuleFor(command => command.DoctorId)
				.NotEmpty().WithMessage("Doctor ID is required.");
		}
	}
}