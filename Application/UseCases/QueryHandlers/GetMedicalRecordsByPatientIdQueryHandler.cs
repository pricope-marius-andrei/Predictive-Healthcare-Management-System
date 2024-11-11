using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetMedicalRecordsByPatientIdQueryHandler : IRequestHandler<GetMedicalRecordsByPatientIdQuery, IEnumerable<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalRecordsByPatientIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalRecordDto>> Handle(GetMedicalRecordsByPatientIdQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _repository.GetByPatientIdAsync(request.PatientId);
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
        }
    }
}


