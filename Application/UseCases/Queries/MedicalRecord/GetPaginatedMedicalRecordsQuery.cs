using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecord
{
    public class GetPaginatedMedicalRecordsQuery : IRequest<Result<PagedResult<MedicalRecordDto>>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}