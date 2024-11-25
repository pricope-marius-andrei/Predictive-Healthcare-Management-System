using Application.DTOs;
using Application.UseCases.Queries.DoctorQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.DoctorQueryHandlers
{
	public class GetDoctorByIdQueryHandler(IDoctorRepository repository, IMapper mapper)
        : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
    {
        public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
		{
			var doctor = await repository.GetByIdAsync(request.DoctorId);

			if (doctor == null)
			{
				return Result<DoctorDto>.Failure("Doctor not found.");
			}

			var doctorDto = mapper.Map<DoctorDto>(doctor);
			return Result<DoctorDto>.Success(doctorDto);
		}
	}
}