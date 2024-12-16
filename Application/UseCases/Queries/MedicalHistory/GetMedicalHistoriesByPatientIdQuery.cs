using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetMedicalHistoriesByPatientIdQuery : IRequest<Result<IEnumerable<MedicalHistoryDto>>>
    {
        public Guid PatientId { get; set; }
    }
}
