using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class DeleteDoctorCommandHandlerTest
    {
        private readonly IDoctorRepository repository;

        public DeleteDoctorCommandHandlerTest()
        {
            repository = Substitute.For<IDoctorRepository>();
        }

        [Fact]
        public async Task Given_DeleteDoctorCommandHandler_When_DoctorExists_Then_DoctorShouldBeDeleted()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var doctor = new Doctor
            {
                DoctorId = doctorId,
                Username = "doctorUsername",
                Email = "doctor@example.com",
                Password = "password",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Specialization = "Cardiology",
                DateOfRegistration = DateTime.Now,
                MedicalRecords = null
            };
            repository.GetByIdAsync(doctorId).Returns(_ => Task.FromResult<Doctor?>(doctor));
            var command = new DeleteDoctorCommand { DoctorId = doctorId };

            // Act
            var handler = new DeleteDoctorCommandHandler(repository);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            await repository.Received(1).DeleteAsync(doctorId);
        }

        [Fact]
        public async Task Given_DeleteDoctorCommandHandler_When_DoctorDoesNotExist_Then_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            repository.GetByIdAsync(doctorId).Returns(_ => Task.FromResult<Doctor?>(null));
            var command = new DeleteDoctorCommand { DoctorId = doctorId };

            // Act
            var handler = new DeleteDoctorCommandHandler(repository);
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Doctor not found.", exception.Message);
        }
    }
}
