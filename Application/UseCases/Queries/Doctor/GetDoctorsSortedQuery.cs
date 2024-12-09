using Application.DTOs;
using MediatR;
using Domain.Common;

namespace Application.UseCases.Queries.Doctor
{
    public class GetDoctorsSortedQuery : IRequest<Result<List<DoctorDto>>>
    {
        public DoctorSortBy SortBy { get; set; }

        public GetDoctorsSortedQuery(DoctorSortBy sortBy)
        {
            SortBy = sortBy;
        }
    }

    public enum DoctorSortBy
    {
        Username,
        FirstName,
        LastName
    }
}