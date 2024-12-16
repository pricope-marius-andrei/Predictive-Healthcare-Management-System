using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecord
{
    public class GetMedicalRecordsByPatientIdQuery : IRequest<Result<IEnumerable<MedicalRecordDto>>>
    {
        public Guid PatientId { get; set; }
    }
}
