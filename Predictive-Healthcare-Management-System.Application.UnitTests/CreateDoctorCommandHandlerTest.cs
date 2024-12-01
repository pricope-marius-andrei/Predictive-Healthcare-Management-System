using Application.UseCases.CommandHandlers.Doctor;
using Application.UseCases.Commands.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class CreateDoctorCommandHandlerTest
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDoctorCommand> _validator;

        public CreateDoctorCommandHandlerTest()
        {
            _repository = Substitute.For<IDoctorRepository>();
            _mapper = Substitute.For<IMapper>();
            _validator = Substitute.For<IValidator<CreateDoctorCommand>>();
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
                Password = "password123",
                Specialization = "Cardiology",
                PhoneNumber = "1234567890",
                DateOfRegistration = DateTime.UtcNow
            };

            var doctor = new Doctor
            {
                DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Username = command.Username,
                Email = command.Email,
                Password = command.Password,
                Specialization = command.Specialization,
                PhoneNumber = command.PhoneNumber,
                DateOfRegistration = command.DateOfRegistration,
                MedicalRecords = new List<MedicalRecord>()
            };

            _validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(new ValidationResult()));
            _mapper.Map<Doctor>(command).Returns(doctor);
            _repository.AddAsync(doctor).Returns(Result<Guid>.Success(doctor.DoctorId));

            // Act
            var handler = new CreateDoctorCommandHandler(_repository, _mapper, _validator);
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

            _validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(validationResult));

            // Act
            var handler = new CreateDoctorCommandHandler(_repository, _mapper, _validator);
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
                Password = "password123",
                Specialization = "Cardiology",
                PhoneNumber = "1234567890",
                DateOfRegistration = DateTime.UtcNow
            };

            var doctor = new Doctor
            {
                DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Username = command.Username,
                Email = command.Email,
                Password = command.Password,
                Specialization = command.Specialization,
                PhoneNumber = command.PhoneNumber,
                DateOfRegistration = command.DateOfRegistration,
                MedicalRecords = new List<MedicalRecord>()

            };

            _validator.ValidateAsync(command, CancellationToken.None).Returns(Task.FromResult(new ValidationResult()));
            _mapper.Map<Doctor>(command).Returns(doctor);
            _repository.AddAsync(doctor).Returns(Result<Guid>.Failure("Error adding doctor"));

            // Act
            var handler = new CreateDoctorCommandHandler(_repository, _mapper, _validator);
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Error adding doctor", result.ErrorMessage);
        }
    }
}
