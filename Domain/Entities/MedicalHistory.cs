using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MedicalHistory
    {
        [Key]
        public Guid MedicalHistoryId { get; set; }

        public Guid PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public string Illness { get; set; }

        public DateTime DateOfDiagnose { get; set; }
    }
}