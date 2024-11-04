using Domain.EnumEntities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public string Gmail { get; set; }

        public DateTime CreatedAt { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
