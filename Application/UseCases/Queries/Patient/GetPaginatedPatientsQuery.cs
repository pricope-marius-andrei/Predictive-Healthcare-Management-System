using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetPaginatedPatientsQuery : IRequest<Result<PagedResult<PatientDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Username { get; set; }
    }
}