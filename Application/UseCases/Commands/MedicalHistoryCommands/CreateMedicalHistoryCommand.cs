using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.MedicalHistoryCommands
{
	public class CreateMedicalHistoryCommand : IRequest<Result<Guid>>
	{
		public Guid PatientId { get; set; }
		public string? Condition { get; set; }
		public DateTime DateOfDiagnosis { get; set; } = DateTime.UtcNow;
	}
}