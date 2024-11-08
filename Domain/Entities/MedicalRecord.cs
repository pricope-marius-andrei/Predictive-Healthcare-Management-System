using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public Guid RecordId { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public string VisitReason { get; set; }

        public string Symptoms { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        public string DoctorNotes { get; set; }
    }
}