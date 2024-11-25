using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PatientQueryHandlers
{
	public class GetAllPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
        : IRequestHandler<GetAllPatientsQuery, Result<IEnumerable<PatientDto>>>
    {
        public async Task<Result<IEnumerable<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
		{
			var patients = await repository.GetAllAsync();
			
			var patientDtos = mapper.Map<IEnumerable<PatientDto>>(patients);
			
			return Result<IEnumerable<PatientDto>>.Success(patientDtos);
		}
	}
}