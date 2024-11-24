using MediatR;

namespace Application.UseCases.Commands.MedicalRecordCommands
{
	public class DeleteMedicalRecordCommand : IRequest
	{
		public Guid RecordId { get; set; }
	}
}