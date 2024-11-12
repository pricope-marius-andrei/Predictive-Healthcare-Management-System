using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Doctor
    {
        [Key]
        public Guid DoctorId { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Specialization { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.Now;

        public required ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
