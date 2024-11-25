using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
{
	public class GetMedicalHistoryByIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        : IRequestHandler<GetMedicalHistoryByIdQuery, Result<MedicalHistoryDto>>
    {
        public async Task<Result<MedicalHistoryDto>> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
		{
			var medicalHistory = await repository.GetByIdAsync(request.HistoryId);

			if (medicalHistory == null)
			{
				return Result<MedicalHistoryDto>.Failure("Medical history not found.");
			}

			var medicalHistoryDto = mapper.Map<MedicalHistoryDto>(medicalHistory);
			return Result<MedicalHistoryDto>.Success(medicalHistoryDto);
		}
	}
}