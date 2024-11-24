using Application.DTOs;
using Application.UseCases.Queries.DoctorQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.DoctorQueryHandlers
{
	public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
	{
		private readonly IDoctorRepository _repository;
		private readonly IMapper _mapper;

		public GetDoctorByIdQueryHandler(IDoctorRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
		{
			var doctor = await _repository.GetByIdAsync(request.DoctorId);

			if (doctor == null)
			{
				return Result<DoctorDto>.Failure("Doctor not found.");
			}

			var doctorDto = _mapper.Map<DoctorDto>(doctor);
			return Result<DoctorDto>.Success(doctorDto);
		}
	}
}