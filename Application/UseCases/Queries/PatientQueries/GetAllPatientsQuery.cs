using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PatientQueries
{
	public class GetAllPatientsQuery : IRequest<Result<IEnumerable<PatientDto>>>
	{

	}
}