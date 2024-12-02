using Application.DTOs;
using MediatR;
using Domain.Common;

namespace Application.UseCases.Queries.MedicalRecord
{
    public class GetMedicalRecordsSortedQuery : IRequest<Result<List<MedicalRecordDto>>>
    {
        public MedicalRecordSortBy SortBy { get; set; }

        public GetMedicalRecordsSortedQuery(MedicalRecordSortBy sortBy)
        {
            SortBy = sortBy;
        }
    }
}