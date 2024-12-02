using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetPaginatedMedicalHistoriesQuery : IRequest<Result<PagedResult<MedicalHistoryDto>>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}