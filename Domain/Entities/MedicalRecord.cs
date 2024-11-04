using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public Guid MedicalRecordId { get; set; }

        public Guid PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public string VisitReason { get; set; }

        public string Symptoms { get; set; }

        public string Diagnosis { get; set; }

        public string DoctorNotes { get; set; }
    }
}