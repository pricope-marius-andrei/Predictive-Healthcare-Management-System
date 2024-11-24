using Application.DTOs;
using Application.UseCases.Queries.DoctorQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.DoctorQueryHandlers
{
	public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, Result<IEnumerable<DoctorDto>>>
	{
		private readonly IDoctorRepository _doctorRepository;
		private readonly IMapper _mapper;

		public GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository, IMapper mapper)
		{
			_doctorRepository = doctorRepository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
		{
			var doctors = await _doctorRepository.GetAllAsync();

			if (doctors == null)
			{
				return Result<IEnumerable<DoctorDto>>.Failure("An error occurred while retrieving doctors.");
			}

			var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

			return Result<IEnumerable<DoctorDto>>.Success(doctorDtos);
		}
	}
}