using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Doctor
    {
        [Key]
        public Guid DoctorId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Specialization { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.Now;

        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
