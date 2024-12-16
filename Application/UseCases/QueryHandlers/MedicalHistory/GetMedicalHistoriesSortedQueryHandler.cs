using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistory
{
    public class GetMedicalHistoriesSortedQueryHandler : IRequestHandler<GetMedicalHistoriesSortedQuery, Result<List<MedicalHistoryDto>>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalHistoriesSortedQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<MedicalHistoryDto>>> Handle(GetMedicalHistoriesSortedQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await _repository.GetAllAsync();

            List<Domain.Entities.MedicalHistory> sortedHistories;

            switch (request.SortBy)
            {
                case MedicalHistorySortBy.DateOfDiagnosis:
                    sortedHistories = medicalHistories.Data.OrderByDescending(m => m.DateOfDiagnosis).ToList();
                    break;
                case MedicalHistorySortBy.Condition:
                    sortedHistories = medicalHistories.Data.OrderBy(m => m.Condition).ToList();
                    break;
                default:
                    return Result<List<MedicalHistoryDto>>.Failure("Invalid sort attribute specified.");
            }

            var medicalHistoryDtos = _mapper.Map<List<MedicalHistoryDto>>(sortedHistories);

            return Result<List<MedicalHistoryDto>>.Success(medicalHistoryDtos);
        }
    }
}