using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetPatientByIdQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _repository.GetByIdAsync(request.Id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }
    }
}