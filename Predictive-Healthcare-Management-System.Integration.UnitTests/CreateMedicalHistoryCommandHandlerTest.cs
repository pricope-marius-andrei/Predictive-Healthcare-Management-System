using Application.UseCases.CommandHandlers.MedicalHistory;
using Application.UseCases.Commands.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class CreateMedicalHistoryCommandHandlerTests
    {
        private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
        private readonly IPatientRepository _mockPatientRepository;
        private readonly IMapper _mockMapper;
        private readonly CreateMedicalHistoryCommandHandler _handler;

        public CreateMedicalHistoryCommandHandlerTests()
        {
            _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
            _mockPatientRepository = Substitute.For<IPatientRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _handler = new CreateMedicalHistoryCommandHandler(
                _mockMedicalHistoryRepository,
                _mockPatientRepository,
                _mockMapper);
        }

        [Fact]
        public async Task Handle_PatientNotFound_ReturnsFailure()
        {
            // Arrange
            var command = new CreateMedicalHistoryCommand { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            _mockPatientRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Patient)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Patient not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var command = new CreateMedicalHistoryCommand
            {
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Condition = "Condition",
                DateOfDiagnosis = DateTime.UtcNow
            };
            var patient = new Patient { PatientId = command.PatientId };
            var medicalHistory = new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

            _mockPatientRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(patient);
            _mockMapper.Map<MedicalHistory>(Arg.Any<CreateMedicalHistoryCommand>()).Returns(medicalHistory);
            _mockMedicalHistoryRepository.AddAsync(Arg.Any<MedicalHistory>()).Returns(Result<Guid>.Success(medicalHistory.HistoryId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(medicalHistory.HistoryId, result.Data);
        }

        [Fact]
        public async Task Handle_AddAsyncFails_ReturnsFailure()
        {
            // Arrange
            var command = new CreateMedicalHistoryCommand
            {
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                Condition = "Condition",
                DateOfDiagnosis = DateTime.UtcNow
            };
            var patient = new Patient { PatientId = command.PatientId };
            var medicalHistory = new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

            _mockPatientRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(patient);
            _mockMapper.Map<MedicalHistory>(Arg.Any<CreateMedicalHistoryCommand>()).Returns(medicalHistory);
            _mockMedicalHistoryRepository.AddAsync(Arg.Any<MedicalHistory>()).Returns(Result<Guid>.Failure("Error adding medical history."));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Error adding medical history.", result.ErrorMessage);
        }
    }
}