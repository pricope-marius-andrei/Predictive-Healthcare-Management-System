using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecord
{
    public class GetMedicalRecordByIdQuery : IRequest<MedicalRecordDto>
    {
        public Guid RecordId { get; set; }
    }
}