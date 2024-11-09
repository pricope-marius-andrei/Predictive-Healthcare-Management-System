using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetMedicalHistoriesByPatientIdQuery : IRequest<IEnumerable<MedicalHistoryDto>>
    {
        public Guid PatientId { get; set; }
    }
}
