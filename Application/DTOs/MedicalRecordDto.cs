﻿namespace Application.DTOs
{
    public class MedicalRecordDto
    {
        public Guid RecordId { get; set; }
        public string VisitReason { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorNotes { get; set; }
        public Guid DoctorId { get; set; }
    }
}