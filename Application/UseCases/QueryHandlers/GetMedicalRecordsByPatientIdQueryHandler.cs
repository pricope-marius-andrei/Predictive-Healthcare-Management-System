using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetMedicalHistoriesByPatientIdQueryHandler : IRequestHandler<GetMedicalHistoriesByPatientIdQuery, IEnumerable<MedicalHistoryDto>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalHistoriesByPatientIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalHistoryDto>> Handle(GetMedicalHistoriesByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await _repository.GetByPatientIdAsync(request.PatientId);
            return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
        }
    }
}

