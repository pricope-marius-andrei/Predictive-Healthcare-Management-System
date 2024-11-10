using MediatR;

namespace Application.UseCases.Commands
{
    public class DeleteMedicalRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
    }
}