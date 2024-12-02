using MediatR;

namespace Application.UseCases.Commands.MedicalHistory
{
    public class DeleteMedicalHistoryCommand : IRequest
    {
        public Guid HistoryId { get; set; }
    }
}