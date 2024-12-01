using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetMedicalHistoryByIdQuery : IRequest<MedicalHistoryDto>
    {
        public Guid HistoryId { get; set; }
    }
}