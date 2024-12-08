using Application.DTOs;
using Application.UseCases.Queries.Patient;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Patient
{
    public class GetPatientsByUsernameFilterQueryHandler : IRequestHandler<GetPatientsByUsernameFilterQuery, IEnumerable<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public GetPatientsByUsernameFilterQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> Handle(GetPatientsByUsernameFilterQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetPatientsByUsernameFilterAsync(request.Username);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
    }
}