using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PatientQueryHandlers
{
	public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
	{
		private readonly IPatientRepository _repository;
		private readonly IMapper _mapper;

		public GetPatientByIdQueryHandler(IPatientRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
		{
			var patient = await _repository.GetByIdAsync(request.PatientId);

			if (patient == null)
			{
				throw new KeyNotFoundException($"Patient with ID {request.PatientId} was not found.");
			}

			var patientDto = _mapper.Map<PatientDto>(patient);

			return Result<PatientDto>.Success(patientDto);
		}
	}
}