﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Patient : User
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public new string? Password { get; set; }

        // public string? Address { get; set; }

        public string? Gender { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public DateTime DateOfRegistration { get; set; } = DateTime.Now;

        public ICollection<MedicalHistory>? MedicalHistories { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}
