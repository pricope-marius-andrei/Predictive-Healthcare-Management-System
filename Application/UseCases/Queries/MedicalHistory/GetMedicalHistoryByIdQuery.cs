using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetMedicalHistoryByIdQuery : IRequest<Result<MedicalHistoryDto>>
    {
        public Guid HistoryId { get; set; }
    }
}