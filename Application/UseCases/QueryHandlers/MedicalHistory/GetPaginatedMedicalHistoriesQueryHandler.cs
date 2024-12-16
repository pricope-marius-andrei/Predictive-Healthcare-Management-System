using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistory
{
    public class GetPaginatedMedicalHistoriesQueryHandler : IRequestHandler<GetPaginatedMedicalHistoriesQuery, Result<PagedResult<MedicalHistoryDto>>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetPaginatedMedicalHistoriesQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<MedicalHistoryDto>>> Handle(GetPaginatedMedicalHistoriesQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await _repository.GetAllAsync();
            var query = medicalHistories.Data.AsQueryable();

            var pagedMedicalHistories = query.ApplyPaging(request.Page, request.PageSize);

            var medicalHistoryDtos = _mapper.Map<List<MedicalHistoryDto>>(pagedMedicalHistories);

            var pagedResult = new PagedResult<MedicalHistoryDto>(medicalHistoryDtos, query.Count());

            return Result<PagedResult<MedicalHistoryDto>>.Success(pagedResult);
        }
    }
}