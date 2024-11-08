using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class CreatePatientCommand : IRequest<Result<Guid>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public ICollection<MedicalHistory>? MedicalHistories { get; set; }
        public ICollection<MedicalRecord>? MedicalRecords { get; set; }
    }
}