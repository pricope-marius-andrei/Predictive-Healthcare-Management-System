using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, Result<IEnumerable<MedicalRecordDto>>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMedicalRecordsQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MedicalRecordDto>>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
        {
            var medicalRecordsResult = await _repository.GetAllAsync();
            if (!medicalRecordsResult.IsSuccess)
            {
                return Result<IEnumerable<MedicalRecordDto>>.Failure(medicalRecordsResult.ErrorMessage);
            }

            var medicalRecordDtos = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecordsResult.Data);
            return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
        }
    }
}

















