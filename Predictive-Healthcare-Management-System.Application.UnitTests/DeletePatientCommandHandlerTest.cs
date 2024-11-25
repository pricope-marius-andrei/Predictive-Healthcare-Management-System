﻿using Application.UseCases.CommandHandlers.PatientCommandHandlers;
using Application.UseCases.Commands.PatientCommands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeletePatientCommandHandlerTests
    {
        private readonly IPatientRepository _mockPatientRepository;
        private readonly DeletePatientCommandHandler _handler;

        public DeletePatientCommandHandlerTests()
        {
            _mockPatientRepository = Substitute.For<IPatientRepository>();
            _handler = new DeletePatientCommandHandler(_mockPatientRepository);
        }

        [Fact]
        public async Task Handle_DeletesPatient_WhenPatientExists()
        {
            // Arrange
            var command = new DeletePatientCommand { PersonId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var patient = new Patient
            {
                PersonId = command.PersonId,
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

            _mockPatientRepository.GetByIdAsync(command.PersonId).Returns(patient);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _mockPatientRepository.Received(1).DeleteAsync(command.PersonId);
        }

        [Fact]
        public async Task Handle_ThrowsInvalidOperationException_WhenPatientDoesNotExist()
        {
            // Arrange
            var command = new DeletePatientCommand { PersonId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

            _mockPatientRepository.GetByIdAsync(command.PersonId).Returns((Patient?)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}