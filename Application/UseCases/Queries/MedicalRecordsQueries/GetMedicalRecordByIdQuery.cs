using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecordsQueries
{
	public class GetMedicalRecordByIdQuery : IRequest<Result<MedicalRecordDto>>
	{
		public Guid RecordId { get; set; }
	}
}