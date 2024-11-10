using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetMedicalRecordsByPatientIdQuery : IRequest<IEnumerable<MedicalRecordDto>>
    {
        public Guid PatientId { get; set; }
    }
}
