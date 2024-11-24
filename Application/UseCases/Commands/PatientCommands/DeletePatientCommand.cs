using MediatR;

namespace Application.UseCases.Commands.PatientCommands
{
	public class DeletePatientCommand : IRequest
	{
		public Guid PersonId { get; set; }
	}
}