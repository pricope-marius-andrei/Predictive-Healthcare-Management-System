using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Xunit;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class CreateDoctorCommandHandlerTest
    {
        private readonly IDoctorRepository repository;
        private readonly IMapper mapper;
        private readonly IValidator<CreateDoctorCommand> validator;

        public CreateDoctorCommandHandlerTest()
        {
            repository = Substitute.For<IDoctorRepository>();
            mapper = Substitute.For<IMapper>();
            validator = Substitute.For<IValidator<CreateDoctorCommand>>();
        }

        [Fact]
        public async Task Given_CreateDoctorCommandHandler_When_CommandIsValid_Then_DoctorShouldBeCreated()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe123",
                Email = "johndoe123@gmail.com",
                Specialization = "Cardiology",
                PhoneNumber = "1234567890",
                DateOfRegistration = DateTime.UtcNow
            };

            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Username = command.Username,
                Email = command.Email,
                Specialization = command.Specialization,
                PhoneNumber = command.PhoneNumber,
                DateOfRegistration = command.DateOfRegistration
            };

            validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(new ValidationResult()));
            mapper.Map<Doctor>(command).Returns(doctor);
            repository.AddAsync(doctor).Returns(Result<Guid>.Success(doctor.DoctorId));

            // Act
            var handler = new CreateDoctorCommandHandler(repository, mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(doctor.DoctorId, result.Data);
        }

        [Fact]
        public async Task Given_CreateDoctorCommandHandler_When_CommandIsInvalid_Then_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateDoctorCommand();
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("FirstName", "First name is required."),
                new ValidationFailure("LastName", "Last name is required.")
            };
            var validationResult = new ValidationResult(validationFailures);

            validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(validationResult));

            // Act
            var handler = new CreateDoctorCommandHandler(repository, mapper, validator);
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal(validationFailures, exception.Errors);
        }

        [Fact]
        public async Task Given_CreateDoctorCommandHandler_When_RepositoryFails_Then_ResultShouldBeFailure()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe123",
                Email = "johndoe123@gmail.com",
                Specialization = "Cardiology",
                PhoneNumber = "1234567890",
                DateOfRegistration = DateTime.UtcNow
            };

            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Username = command.Username,
                Email = command.Email,
                Specialization = command.Specialization,
                PhoneNumber = command.PhoneNumber,
                DateOfRegistration = command.DateOfRegistration
            };

            validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(new ValidationResult()));
            mapper.Map<Doctor>(command).Returns(doctor);
            repository.AddAsync(doctor).Returns(Result<Guid>.Failure("Error adding doctor"));

            // Act
            var handler = new CreateDoctorCommandHandler(repository, mapper, validator);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Error adding doctor", result.ErrorMessage);
        }
    }
}
