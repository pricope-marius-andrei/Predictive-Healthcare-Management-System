namespace Application.DTOs
{
    internal class MedicalRecordDto
    {
        public Guid MedicalRecordId { get; set; }
        public Guid PatientId { get; set; }
        public string VisitReason { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorNotes { get; set; }
    }
}
