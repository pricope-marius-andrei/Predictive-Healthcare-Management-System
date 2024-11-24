using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public abstract class Person
	{
		[Key]
		public Guid PersonId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(6)]
		public string Password { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[Phone]
		[MaxLength(20)]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
		public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;
	}
}