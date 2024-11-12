using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommand : IRequest<Result<Guid>>
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public required List<MedicalRecord> MedicalRecords { get; set; }
    }
}