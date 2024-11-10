using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetMedicalRecordByIdQuery : IRequest<MedicalRecordDto>
    {
        public Guid RecordId { get; set; }
    }
}