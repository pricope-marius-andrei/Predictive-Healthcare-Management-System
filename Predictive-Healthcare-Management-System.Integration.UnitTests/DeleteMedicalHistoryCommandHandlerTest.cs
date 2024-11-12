using Application.UseCases.CommandHandlers;
using Application.UseCases.Commands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

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
        var command = new DeleteMedicalHistoryCommand { HistoryId = Guid.NewGuid() };
        _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns((MedicalHistory)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Medical history not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesMedicalHistory()
    {
        // Arrange
        var command = new DeleteMedicalHistoryCommand { HistoryId = Guid.NewGuid() };
        var medicalHistory = new MedicalHistory
        {
            HistoryId = command.HistoryId,
            PatientId = Guid.NewGuid(),
            Condition = "Condition",
            DateOfDiagnosis = DateTime.UtcNow
        };

        var doctor = new Doctor
        {
            DoctorId = Guid.NewGuid(),
            Username = "doctorUsername",
            Email = "doctor@example.com",
            Password = "password123",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Specialization = "Cardiology",
            DateOfRegistration = DateTime.UtcNow,
            MedicalRecords = new List<MedicalRecord>()
        };

        _mockMedicalHistoryRepository.GetByIdAsync(command.HistoryId).Returns(medicalHistory);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _mockMedicalHistoryRepository.Received(1).DeleteAsync(command.HistoryId);
    }
}
