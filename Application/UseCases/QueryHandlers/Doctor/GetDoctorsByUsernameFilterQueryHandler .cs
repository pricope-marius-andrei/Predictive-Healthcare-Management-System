using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetDoctorsByUsernameFilterQueryHandler : IRequestHandler<GetDoctorsByUsernameFilterQuery, IEnumerable<DoctorDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorsByUsernameFilterQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> Handle(GetDoctorsByUsernameFilterQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetDoctorsByUsernameFilterAsync(request.Username);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}