using Application.UseCases.CommandHandlers.Doctor;
using Application.UseCases.Commands.Doctor;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeleteDoctorCommandHandlerTest
    {
        private readonly IDoctorRepository _repository;

        public DeleteDoctorCommandHandlerTest()
        {
            _repository = Substitute.For<IDoctorRepository>();
        }

        [Fact]
        public async Task Given_DeleteDoctorCommandHandler_When_DoctorExists_Then_DoctorShouldBeDeleted()
        {
            // Arrange
            var doctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf");
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
                MedicalRecords = new List<MedicalRecord>()
            };
            _repository.GetByIdAsync(doctorId).Returns(_ => Task.FromResult<Doctor>(doctor));
            var command = new DeleteDoctorCommand { DoctorId = doctorId };

            // Act
            var handler = new DeleteDoctorCommandHandler(_repository);
            await handler.Handle(command, CancellationToken.None);

            // Assert
            await _repository.Received(1).DeleteAsync(doctorId);
        }

        [Fact]
        public async Task Given_DeleteDoctorCommandHandler_When_DoctorDoesNotExist_Then_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var doctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf");
            _repository.GetByIdAsync(doctorId).Returns(_ => Task.FromResult<Doctor>(null!));

            var command = new DeleteDoctorCommand { DoctorId = doctorId };

            // Act
            var handler = new DeleteDoctorCommandHandler(_repository);
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Doctor not found.", exception.Message);
        }
    }
}
