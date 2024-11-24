using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.MedicalRecordCommands
{
	public class CreateMedicalRecordCommand : IRequest<Result<Guid>>
	{
		public Guid PatientId { get; set; }
		public Guid DoctorId { get; set; }
		public string? VisitReason { get; set; }
		public string? Symptoms { get; set; }
		public string? Diagnosis { get; set; }
		public string? DoctorNotes { get; set; }
		public DateTime DateOfVisit { get; set; } = DateTime.UtcNow;
	}
}