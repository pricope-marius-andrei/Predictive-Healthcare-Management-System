using Domain.Entities;

namespace Application.DTOs
{
    public class PatientsDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public ICollection<MedicalHistory> MedicalHistories { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
