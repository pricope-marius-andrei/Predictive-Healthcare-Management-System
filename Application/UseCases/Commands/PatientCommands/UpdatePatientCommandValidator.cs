using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCases.Commands.PatientCommands
{
	public class UpdatePatientCommandValidator : BasePatientCommandValidator<UpdatePatientCommand, Result<Patient>>
	{
		public UpdatePatientCommandValidator() : base()
		{
			RuleFor(command => command.PatientId)
				.NotEmpty().WithMessage("Patient ID is required.");
		}
	}
}