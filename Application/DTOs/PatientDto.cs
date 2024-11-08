namespace Application.DTOs
{
    public class PatientDto
    {
        public Guid PatientId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public ICollection<MedicalHistoryDto> MedicalHistories { get; set; }
        public ICollection<MedicalRecordDto> MedicalRecords { get; set; }
    }
}
