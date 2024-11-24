using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistoryQueries
{
	public class GetAllMedicalHistoriesQuery : IRequest<Result<IEnumerable<MedicalHistoryDto>>>
	{
		
	}
}