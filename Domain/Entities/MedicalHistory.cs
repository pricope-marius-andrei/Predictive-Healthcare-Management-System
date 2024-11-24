using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	public class MedicalHistory
	{
		[Key]
		public Guid HistoryId { get; set; }

		[Required]
		public Guid PatientId { get; set; }

		[ForeignKey("PatientId")]
		public Patient? Patient { get; set; }

		[Required]
		[MaxLength(200)]
		public string Condition { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfDiagnosis { get; set; } = DateTime.UtcNow;
	}
}