using Application.UseCases.CommandHandlers.Patient;
using Application.UseCases.Commands.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class CreatePatientCommandHandlerTests
    {
        private readonly IPatientRepository _mockPatientRepository;
        private readonly IMapper _mockMapper;
        private readonly IValidator<CreatePatientCommand> _mockValidator;
        private readonly CreatePatientCommandHandler _handler;

        public CreatePatientCommandHandlerTests()
        {
            _mockPatientRepository = Substitute.For<IPatientRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _mockValidator = Substitute.For<IValidator<CreatePatientCommand>>();
            _handler = new CreatePatientCommandHandler(_mockPatientRepository, _mockMapper, _mockValidator);
        }

        [Fact]
        public async Task Handle_ReturnsSuccessResult_WhenPatientIsCreated()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                Address = "123 Test St",
                Gender = "Male",
                Height = 180,
                Weight = 75,
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateOfRegistration = DateTime.UtcNow
            };
            var patient = new Patient
            {
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Username = command.Username,
                Email = command.Email,
                Password = command.Password,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                Address = command.Address,
                Gender = command.Gender,
                Height = command.Height.Value,
                Weight = command.Weight.Value,
                DateOfBirth = command.DateOfBirth,
                DateOfRegistration = command.DateOfRegistration
            };
            var result = Result<Guid>.Success(patient.PatientId);

            _mockValidator.ValidateAsync(command, CancellationToken.None).Returns(new ValidationResult());
            _mockMapper.Map<Patient>(command).Returns(patient);
            _mockPatientRepository.AddAsync(patient).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(result.Data, response.Data);
        }

        [Fact]
        public async Task Handle_ThrowsValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreatePatientCommand();
            var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Username", "Username is required")
        };
            var validationResult = new ValidationResult(validationFailures);

            _mockValidator.ValidateAsync(command, CancellationToken.None).Returns(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ReturnsFailureResult_WhenRepositoryFailsToAddPatient()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                Address = "123 Test St",
                Gender = "Male",
                Height = 180,
                Weight = 75,
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateOfRegistration = DateTime.UtcNow
            };
            var patient = new Patient
            {
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Username = command.Username,
                Email = command.Email,
                Password = command.Password,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                Address = command.Address,
                Gender = command.Gender,
                Height = command.Height.Value,
                Weight = command.Weight.Value,
                DateOfBirth = command.DateOfBirth,
                DateOfRegistration = command.DateOfRegistration
            };
            var result = Result<Guid>.Failure("Failed to add patient");

            _mockValidator.ValidateAsync(command, CancellationToken.None).Returns(new ValidationResult());
            _mockMapper.Map<Patient>(command).Returns(patient);
            _mockPatientRepository.AddAsync(patient).Returns(result);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal(result.ErrorMessage, response.ErrorMessage);
        }
    }
}