using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistory
{
    public class GetMedicalHistoriesByPatientIdQueryHandler : IRequestHandler<GetMedicalHistoriesByPatientIdQuery, Result<IEnumerable<MedicalHistoryDto>>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalHistoriesByPatientIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MedicalHistoryDto>>> Handle(GetMedicalHistoriesByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var medicalHistoriesResult = await _repository.GetByPatientIdAsync(request.PatientId);
            if (!medicalHistoriesResult.IsSuccess)
            {
                return Result<IEnumerable<MedicalHistoryDto>>.Failure(medicalHistoriesResult.ErrorMessage);
            }

            var medicalHistoryDtos = _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistoriesResult.Data);
            return Result<IEnumerable<MedicalHistoryDto>>.Success(medicalHistoryDtos);
        }
    }
}














