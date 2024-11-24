using MediatR;

namespace Application.UseCases.Commands.MedicalHistoryCommands
{
	public class DeleteMedicalHistoryCommand : IRequest
	{
		public Guid HistoryId { get; set; }
	}
}