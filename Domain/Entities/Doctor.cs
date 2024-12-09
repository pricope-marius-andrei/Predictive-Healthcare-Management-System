using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Doctor : User
    {
        [Required]
        public string? Specialization { get; set; }
        public List<Patient>? Patients { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}