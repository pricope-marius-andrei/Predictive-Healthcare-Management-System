using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Domain.Common;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetMedicalRecordsSortedQueryHandler : IRequestHandler<GetMedicalRecordsSortedQuery, Result<List<MedicalRecordDto>>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalRecordsSortedQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<MedicalRecordDto>>> Handle(GetMedicalRecordsSortedQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _repository.GetAllAsync();

            List<Domain.Entities.MedicalRecord> sortedRecords;

            switch (request.SortBy)
            {
                case MedicalRecordSortBy.VisitReason:
                    sortedRecords = medicalRecords.Data.OrderBy(m => m.VisitReason).ToList();
                    break;
                case MedicalRecordSortBy.Symptoms:
                    sortedRecords = medicalRecords.Data.OrderBy(m => m.Symptoms).ToList();
                    break;
                default:
                    return Result<List<MedicalRecordDto>>.Failure("Invalid sort attribute specified.");
            }

            var medicalRecordDtos = _mapper.Map<List<MedicalRecordDto>>(sortedRecords);

            return Result<List<MedicalRecordDto>>.Success(medicalRecordDtos);
        }
    }
}