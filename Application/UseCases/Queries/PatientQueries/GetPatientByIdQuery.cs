using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.PatientQueries
{
	public class GetPatientByIdQuery : IRequest<Result<PatientDto>>
	{
		public Guid PatientId { get; set; }
	}
}