using Application.UseCases.CommandHandlers;
using Application.UseCases.Commands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

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
        var command = new DeleteMedicalRecordCommand { RecordId = Guid.NewGuid() };
        _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns((MedicalRecord)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Medical record not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesMedicalRecord()
    {
        // Arrange
        var command = new DeleteMedicalRecordCommand { RecordId = Guid.NewGuid() };
        var medicalRecord = new MedicalRecord
        {
            RecordId = command.RecordId,
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            VisitReason = "Checkup",
            Diagnosis = "Healthy",
            DateOfVisit = DateTime.UtcNow
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

        _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(medicalRecord);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _mockMedicalRecordRepository.Received(1).DeleteAsync(command.RecordId);
    }
}
