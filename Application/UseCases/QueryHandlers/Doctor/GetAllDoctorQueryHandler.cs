using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorsQuery, Result<IEnumerable<DoctorDto>>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDoctorQueryHandler(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctorsResult = await _repository.GetAllAsync();
            if (!doctorsResult.IsSuccess)
            {
                return Result<IEnumerable<DoctorDto>>.Failure(doctorsResult.ErrorMessage);
            }

            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctorsResult.Data);
            return Result<IEnumerable<DoctorDto>>.Success(doctorDtos);
        }
    }
}










