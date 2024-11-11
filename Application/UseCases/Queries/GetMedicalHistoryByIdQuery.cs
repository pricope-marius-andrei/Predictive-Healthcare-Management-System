using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetMedicalHistoryByIdQuery : IRequest<MedicalHistoryDto>
    {
        public Guid HistoryId { get; set; }
    }
}