using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
{
	public class GetMedicalHistoriesByPatientIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        : IRequestHandler<GetMedicalHistoriesByPatientIdQuery, Result<IEnumerable<MedicalHistoryDto>>>
    {
        public async Task<Result<IEnumerable<MedicalHistoryDto>>> Handle(GetMedicalHistoriesByPatientIdQuery request, CancellationToken cancellationToken)
		{
			var medicalHistories = await repository.GetByPatientIdAsync(request.PatientId);

			if (medicalHistories == null || !medicalHistories.Any())
			{
				return Result<IEnumerable<MedicalHistoryDto>>.Failure("No medical histories found for the specified patient.");
			}

			var medicalHistoryDtos = mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
			return Result<IEnumerable<MedicalHistoryDto>>.Success(medicalHistoryDtos);
		}
	}
}