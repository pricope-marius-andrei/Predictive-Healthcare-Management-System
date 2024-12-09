using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _repository;

        public RegisterUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            User user = CreateUserEntity(request);

            await _repository.Register(user, cancellationToken);

            return user.Id;
        }

        private void ValidateRequest(RegisterUserCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new ArgumentException("First name is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new ArgumentException("Last name is required.");

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                throw new ArgumentException("Phone number is required.");
        }

        private User CreateUserEntity(RegisterUserCommand request)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            switch (request.UserType)
            {
                case EUserType.Doctor:

                    return new Doctor
                    {
                        Email = request.Email,
                        Password = hashedPassword,
                        Username = request.Username,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber
                    };

                case EUserType.Patient:

                    return new Patient
                    {
                        Email = request.Email,
                        Password = hashedPassword,
                        Username = request.Username,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber
                    };

                default:
                    throw new InvalidOperationException("Invalid user type specified.");
            }
        }
    }
}