using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetPaginatedDoctorsQuery : IRequest<Result<PagedResult<DoctorDto>>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}