using Application.DTOs;
using MediatR;
using Domain.Common;

namespace Application.UseCases.Queries.MedicalHistory
{
    public class GetMedicalHistoriesSortedQuery : IRequest<Result<List<MedicalHistoryDto>>>
    {
        public MedicalHistorySortBy SortBy { get; set; }

        public GetMedicalHistoriesSortedQuery(MedicalHistorySortBy sortBy)
        {
            SortBy = sortBy;
        }
    }

    public enum MedicalHistorySortBy
    {
        DateOfDiagnosis,
        Condition
    }
}