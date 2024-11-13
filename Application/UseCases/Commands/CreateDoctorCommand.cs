using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommand : IRequest<Result<Guid>>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public List<MedicalRecord>? MedicalRecords { get; set; }
    }
}