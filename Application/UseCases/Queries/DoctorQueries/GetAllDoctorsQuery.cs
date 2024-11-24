using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.DoctorQueries
{
	public class GetAllDoctorsQuery : IRequest<Result<IEnumerable<DoctorDto>>>
	{

	}
}