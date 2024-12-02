using MediatR;

namespace Application.UseCases.Commands.MedicalRecord
{
    public class DeleteMedicalRecordCommand : IRequest
    {
        public Guid RecordId { get; set; }
    }
}