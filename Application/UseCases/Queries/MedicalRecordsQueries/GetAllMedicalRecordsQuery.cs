using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecordsQueries
{
	public class GetAllMedicalRecordsQuery : IRequest<Result<IEnumerable<MedicalRecordDto>>>
	{
		
	}
}