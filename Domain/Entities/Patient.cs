using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class Patient : Person
	{
		[Required]
		public DateTime DateOfBirth { get; set; }

		[MaxLength(100)]
		public string? Address { get; set; }

		[MaxLength(10)]
		public string? Gender { get; set; }

		[Range(0, 300)]
		public decimal Height { get; set; }

		[Range(0, 500)]
		public decimal Weight { get; set; }

		public ICollection<MedicalHistory>? MedicalHistories { get; set; }

		public ICollection<MedicalRecord>? MedicalRecords { get; set; }
	}
}