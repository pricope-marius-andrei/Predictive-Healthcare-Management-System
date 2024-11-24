using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.PatientCommands
{
	public class CreatePatientCommand : BasePatientCommand<Result<Guid>>, IRequest<Result<Guid>>
	{

	}
}