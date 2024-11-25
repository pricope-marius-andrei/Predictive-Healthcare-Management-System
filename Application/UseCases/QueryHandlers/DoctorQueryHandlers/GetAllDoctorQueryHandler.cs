using Application.DTOs;
using Application.UseCases.Queries.DoctorQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.DoctorQueryHandlers
{
	public class GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
        : IRequestHandler<GetAllDoctorsQuery, Result<IEnumerable<DoctorDto>>>
    {
        public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
		{
			var doctors = await doctorRepository.GetAllAsync();

			if (doctors == null)
			{
				return Result<IEnumerable<DoctorDto>>.Failure("An error occurred while retrieving doctors.");
			}

			var doctorDtos = mapper.Map<IEnumerable<DoctorDto>>(doctors);

			return Result<IEnumerable<DoctorDto>>.Success(doctorDtos);
		}
	}
}