using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.MedicalHistoryQueries
{
	public class GetMedicalHistoriesByPatientIdQuery : IRequest<Result<IEnumerable<MedicalHistoryDto>>>
	{
		public Guid PatientId { get; set; }
	}
}