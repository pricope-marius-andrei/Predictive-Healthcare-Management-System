namespace Application.DTOs
{
    public class MedicalHistoryDto
    {
        public Guid HistoryId { get; set; }
        public DateTime DateOfDiagnosis { get; set; }
        public Guid PatientId { get; set; }
        public PatientDto? Patient { get; set; }
        public string? Condition { get; set; }
    }
}
