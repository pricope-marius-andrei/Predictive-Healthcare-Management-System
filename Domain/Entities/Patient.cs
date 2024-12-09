using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Patient : User
    {
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public Guid? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<MedicalHistory>? MedicalHistories { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}
