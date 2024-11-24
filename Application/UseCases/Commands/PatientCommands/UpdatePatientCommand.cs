using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.PatientCommands
{
	public class UpdatePatientCommand : BasePatientCommand<Result<Patient>>, IRequest<Result<Patient>>
	{
		public Guid PatientId { get; set; }
	}
}