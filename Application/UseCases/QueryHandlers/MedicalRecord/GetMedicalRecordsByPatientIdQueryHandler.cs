using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetMedicalRecordsByPatientIdQueryHandler : IRequestHandler<GetMedicalRecordsByPatientIdQuery, Result<IEnumerable<MedicalRecordDto>>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalRecordsByPatientIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MedicalRecordDto>>> Handle(GetMedicalRecordsByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var medicalRecordsResult = await _repository.GetByPatientIdAsync(request.PatientId);
            if (!medicalRecordsResult.IsSuccess)
            {
                return Result<IEnumerable<MedicalRecordDto>>.Failure(medicalRecordsResult.ErrorMessage);
            }

            var medicalRecordDtos = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecordsResult.Data);
            return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
        }
    }
}




















