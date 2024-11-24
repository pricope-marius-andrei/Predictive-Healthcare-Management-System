using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PatientQueryHandlers
{
	public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, Result<IEnumerable<PatientDto>>>
	{
		private readonly IPatientRepository _repository;
		private readonly IMapper _mapper;

		public GetAllPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
		{
			var patients = await _repository.GetAllAsync();
			
			var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
			
			return Result<IEnumerable<PatientDto>>.Success(patientDtos);
		}
	}
}