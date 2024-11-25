using Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers;
using Application.UseCases.Commands.MedicalRecordCommands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeleteMedicalRecordCommandHandlerTests
    {
        private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
        private readonly DeleteMedicalRecordCommandHandler _handler;

        public DeleteMedicalRecordCommandHandlerTests()
        {
            _mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
            _handler = new DeleteMedicalRecordCommandHandler(_mockMedicalRecordRepository);
        }

        [Fact]
        public async Task Handle_MedicalRecordNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new DeleteMedicalRecordCommand { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns((MedicalRecord?)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Medical record not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesMedicalRecord()
        {
            // Arrange
            var command = new DeleteMedicalRecordCommand { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var medicalRecord = new MedicalRecord
            {
                RecordId = command.RecordId,
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                VisitReason = "Checkup",
                Diagnosis = "Healthy",
                DateOfVisit = DateTime.UtcNow
            };

            _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(medicalRecord);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _mockMedicalRecordRepository.Received(1).DeleteAsync(command.RecordId);
        }
    }
}