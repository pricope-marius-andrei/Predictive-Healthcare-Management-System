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
        public Guid HistoryId { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public string Condition { get; set; }
        public DateTime DateOfDiagnosis { get; set; } = DateTime.Now;
    }
}