using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetDoctorsByUsernameFilterQueryHandler : IRequestHandler<GetDoctorsByUsernameFilterQuery, Result<IEnumerable<DoctorDto>>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorsByUsernameFilterQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetDoctorsByUsernameFilterQuery request, CancellationToken cancellationToken)
        {
            var doctorsResult = await _doctorRepository.GetDoctorsByUsernameFilterAsync(request.Username);
            if (!doctorsResult.IsSuccess)
            {
                return Result<IEnumerable<DoctorDto>>.Failure(doctorsResult.ErrorMessage);
            }

            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctorsResult.Data);
            return Result<IEnumerable<DoctorDto>>.Success(doctorDtos);
        }
    }
}












