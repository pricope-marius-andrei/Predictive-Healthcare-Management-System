using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetAllMedicalHistoriesQuery : IRequest<IEnumerable<MedicalHistoryDto>>
    {
    }
}