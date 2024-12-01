using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetPatientsSortedQuery : IRequest<Result<List<PatientDto>>>
    {
        public PatientSortBy SortBy { get; set; }

        public GetPatientsSortedQuery(PatientSortBy sortBy)
        {
            SortBy = sortBy;
        }
    }
}