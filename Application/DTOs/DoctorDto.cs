﻿namespace Application.DTOs
{
    public class DoctorDto
    {
        public Guid DoctorId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public ICollection<PatientDto>? Patients { get; set; }
        public ICollection<MedicalRecordDto>? MedicalRecords { get; set; }
    }

    public class AssignPatientToDoctorRequest
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
    }

    public class RemovePatientFromDoctorRequest
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
    }
}