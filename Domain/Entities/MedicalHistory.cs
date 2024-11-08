using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MedicalHistory
    {
        [Key]
        public Guid HistoryId { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public string Condition { get; set; }
        public DateTime DateOfDiagnosis { get; set; } = DateTime.Now;
    }
}