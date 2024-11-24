using Application.UseCases.CommandHandlers.MedicalHistoryCommandHandlers;
using Application.UseCases.Commands.MedicalHistoryCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class UpdateMedicalHistoryCommandHandlerTests
    {
        private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
        private readonly IMapper _mockMapper;
        private readonly UpdateMedicalHistoryCommandHandler _handler;

        public UpdateMedicalHistoryCommandHandlerTests()
        {
            _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _handler = new UpdateMedicalHistoryCommandHandler(_mockMedicalHistoryRepository, _mockMapper);
        }

        [Fact]
        public async Task Handle_MedicalHistoryNotFound_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateMedicalHistoryCommand { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns((MedicalHistory?)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Medical history not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesMedicalHistory()
        {
            // Arrange
            var command = new UpdateMedicalHistoryCommand
            {
                HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Condition = "Updated Condition",
                DateOfDiagnosis = DateTime.UtcNow
            };

            var existingMedicalHistory = new MedicalHistory
            {
                HistoryId = command.HistoryId,
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Condition = "Condition",
                DateOfDiagnosis = DateTime.UtcNow
            };

            var updatedMedicalHistory = new MedicalHistory
            {
                HistoryId = command.HistoryId,
                PatientId = existingMedicalHistory.PatientId,
                Condition = command.Condition,
                DateOfDiagnosis = command.DateOfDiagnosis
            };

            _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns(existingMedicalHistory);
            _mockMapper.Map<MedicalHistory>(command).Returns(updatedMedicalHistory);
            _mockMedicalHistoryRepository.UpdateAsync(updatedMedicalHistory).Returns(Result<MedicalHistory>.Success(updatedMedicalHistory));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updatedMedicalHistory, result.Data);
        }
    }
}