using MediatR;

namespace Application.UseCases.Authentication
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public EUserType UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum EUserType
    {
        Patient,
        Doctor,
        Admin
    }
}