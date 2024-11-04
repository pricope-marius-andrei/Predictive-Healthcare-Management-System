namespace Application.DTOs
{
    internal class MedicalHistoryDto
    {
        public Guid MedicalHistoryId { get; set; }
        public Guid PatientId { get; set; }
        public string Illness { get; set; }
        public DateTime DateOfDiagnose { get; set; }
    }
}
