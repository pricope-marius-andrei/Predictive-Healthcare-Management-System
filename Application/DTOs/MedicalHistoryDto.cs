namespace Application.DTOs
{
    public class MedicalHistoryDto
    {
        public Guid HistoryId { get; set; }
        public string Illness { get; set; }
        public DateTime DateOfDiagnosis { get; set; }
    }
}
