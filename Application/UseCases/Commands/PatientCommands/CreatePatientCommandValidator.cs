using Application.UseCases.Commands.BaseCommands;
using Domain.Common;

namespace Application.UseCases.Commands.PatientCommands
{
	public class CreatePatientCommandValidator : BaseUserCommandValidator<CreatePatientCommand, Result<Guid>>
	{
		public CreatePatientCommandValidator() : base()
		{

		}
	}
}