using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetMedicalRecordByIdQueryHandler : IRequestHandler<GetMedicalRecordByIdQuery, Result<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalRecordByIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<MedicalRecordDto>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var medicalRecordResult = await _repository.GetByIdAsync(request.RecordId);
            if (!medicalRecordResult.IsSuccess)
            {
                return Result<MedicalRecordDto>.Failure(medicalRecordResult.ErrorMessage);
            }

            var medicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecordResult.Data);
            return Result<MedicalRecordDto>.Success(medicalRecordDto);
        }
    }
}
















