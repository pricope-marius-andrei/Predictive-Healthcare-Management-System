using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.DoctorQueries
{
	public class GetDoctorByIdQuery : IRequest<Result<DoctorDto>>
	{
		public Guid DoctorId { get; set; }
	}
}