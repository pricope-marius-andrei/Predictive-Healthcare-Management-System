using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	public class MedicalRecord
	{
		[Key]
		public Guid RecordId { get; set; }

		[Required]
		public Guid PatientId { get; set; }

		[ForeignKey("PatientId")]
		public Patient? Patient { get; set; }

		[Required]
		public Guid DoctorId { get; set; }

		[ForeignKey("DoctorId")]
		public Doctor? Doctor { get; set; }

		[Required]
		[MaxLength(200)]
		public string VisitReason { get; set; } = string.Empty;

		[MaxLength(1000)]
		public string? Symptoms { get; set; }

		[Required]
		[MaxLength(500)]
		public string Diagnosis { get; set; } = string.Empty;

		[MaxLength(1000)]
		public string? DoctorNotes { get; set; }

		[Required]
		public DateTime DateOfVisit { get; set; } = DateTime.UtcNow;
	}
}