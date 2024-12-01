using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetPaginatedMedicalRecordsQueryHandler : IRequestHandler<GetPaginatedMedicalRecordsQuery, Result<PagedResult<MedicalRecordDto>>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetPaginatedMedicalRecordsQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<MedicalRecordDto>>> Handle(GetPaginatedMedicalRecordsQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _repository.GetAllAsync();
            var query = medicalRecords.AsQueryable();

            var pagedMedicalRecords = query.ApplyPaging(request.Page, request.PageSize);

            var medicalRecordDtos = _mapper.Map<List<MedicalRecordDto>>(pagedMedicalRecords);

            var pagedResult = new PagedResult<MedicalRecordDto>(medicalRecordDtos, query.Count());

            return Result<PagedResult<MedicalRecordDto>>.Success(pagedResult);
        }
    }
}