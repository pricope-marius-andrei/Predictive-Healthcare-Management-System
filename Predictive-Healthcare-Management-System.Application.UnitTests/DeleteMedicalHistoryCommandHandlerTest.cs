using Application.UseCases.CommandHandlers.MedicalHistoryCommandHandlers;
using Application.UseCases.Commands.MedicalHistoryCommands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeleteMedicalHistoryCommandHandlerTests
    {
        private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
        private readonly DeleteMedicalHistoryCommandHandler _handler;

        public DeleteMedicalHistoryCommandHandlerTests()
        {
            _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
            _handler = new DeleteMedicalHistoryCommandHandler(_mockMedicalHistoryRepository);
        }

        [Fact]
        public async Task Handle_MedicalHistoryNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new DeleteMedicalHistoryCommand { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns((MedicalHistory)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Medical history not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesMedicalHistory()
        {
            // Arrange
            var command = new DeleteMedicalHistoryCommand { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var medicalHistory = new MedicalHistory
            {
                HistoryId = command.HistoryId,
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Condition = "Condition",
                DateOfDiagnosis = DateTime.UtcNow
            };

            _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns(medicalHistory);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _mockMedicalHistoryRepository.Received(1).DeleteAsync(command.HistoryId);
        }
    }
}