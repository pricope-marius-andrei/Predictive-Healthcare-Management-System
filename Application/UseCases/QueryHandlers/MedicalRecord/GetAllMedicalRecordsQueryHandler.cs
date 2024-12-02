using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecord
{
    public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, IEnumerable<MedicalRecordDto>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMedicalRecordsQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalRecordDto>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
        {
            var medicalRecords = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
        }
    }
}



