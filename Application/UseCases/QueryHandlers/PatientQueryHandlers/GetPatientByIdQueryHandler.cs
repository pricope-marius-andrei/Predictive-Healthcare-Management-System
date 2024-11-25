using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PatientQueryHandlers
{
	public class GetPatientByIdQueryHandler(IPatientRepository repository, IMapper mapper)
        : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
    {
        public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
		{
			var patient = await repository.GetByIdAsync(request.PatientId);

			if (patient == null)
			{
				throw new KeyNotFoundException($"Patient with ID {request.PatientId} was not found.");
			}

			var patientDto = mapper.Map<PatientDto>(patient);

			return Result<PatientDto>.Success(patientDto);
		}
	}
}