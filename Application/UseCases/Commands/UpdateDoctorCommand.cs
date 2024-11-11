using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommand : IRequest<Result<Doctor>>
    {
        public Guid DoctorId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string PhoneNumber { get; set; }
    }
}
