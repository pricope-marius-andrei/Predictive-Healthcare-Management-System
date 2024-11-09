using MediatR;

namespace Application.UseCases.Commands
{
    public class DeleteMedicalHistoryCommand : IRequest
    {
        public Guid HistoryId { get; set; }
    }
}