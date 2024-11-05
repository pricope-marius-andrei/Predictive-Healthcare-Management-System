using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Patient
    {
        [Key, ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public ICollection<MedicalHistory> MedicalHistories { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
