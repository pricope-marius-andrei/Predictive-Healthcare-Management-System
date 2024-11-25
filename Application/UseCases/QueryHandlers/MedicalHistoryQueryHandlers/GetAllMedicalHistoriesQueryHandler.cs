using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
{
	public class GetAllMedicalHistoriesQueryHandler(IMedicalHistoryRepository medicalHistoryRepository, IMapper mapper)
        : IRequestHandler<GetAllMedicalHistoriesQuery, Result<IEnumerable<MedicalHistoryDto>>>
    {
        public async Task<Result<IEnumerable<MedicalHistoryDto>>> Handle(GetAllMedicalHistoriesQuery request, CancellationToken cancellationToken)
		{
			var medicalHistories = await medicalHistoryRepository.GetAllAsync();

			var medicalHistoryDtos = mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);

			return Result<IEnumerable<MedicalHistoryDto>>.Success(medicalHistoryDtos);
		}
	}
}