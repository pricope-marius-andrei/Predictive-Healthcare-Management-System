using Application.UseCases.CommandHandlers;
using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;
using NSubstitute;
using Xunit;

public class UpdateMedicalRecordCommandHandlerTests
{
    private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
    private readonly IMapper _mockMapper;
    private readonly UpdateMedicalRecordCommandHandler _handler;

    public UpdateMedicalRecordCommandHandlerTests()
    {
        _mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new UpdateMedicalRecordCommandHandler(_mockMedicalRecordRepository, _mockMapper);
    }

    [Fact]
    public async Task Handle_MedicalRecordNotFound_ReturnsFailureResult()
    {
        // Arrange
        var command = new UpdateMedicalRecordCommand { RecordId = Guid.NewGuid() };
        _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns((MedicalRecord)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Medical record not found.", result.ErrorMessage);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesMedicalRecord()
    {
        // Arrange
        var command = new UpdateMedicalRecordCommand
        {
            RecordId = Guid.NewGuid(),
            VisitReason = "Updated Visit Reason",
            Symptoms = "Updated Symptoms",
            Diagnosis = "Updated Diagnosis",
            DoctorNotes = "Updated Doctor Notes",
            DateOfVisit = DateTime.UtcNow
        };

        var existingMedicalRecord = new MedicalRecord
        {
            RecordId = command.RecordId,
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            VisitReason = "Visit Reason",
            Symptoms = "Symptoms",
            Diagnosis = "Diagnosis",
            DoctorNotes = "Doctor Notes",
            DateOfVisit = DateTime.UtcNow
        };

        var updatedMedicalRecord = new MedicalRecord
        {
            RecordId = command.RecordId,
            PatientId = existingMedicalRecord.PatientId,
            DoctorId = existingMedicalRecord.DoctorId,
            VisitReason = command.VisitReason,
            Symptoms = command.Symptoms,
            Diagnosis = command.Diagnosis,
            DoctorNotes = command.DoctorNotes,
            DateOfVisit = command.DateOfVisit
        };

        _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(existingMedicalRecord);
        _mockMapper.Map<MedicalRecord>(command).Returns(updatedMedicalRecord);
        _mockMedicalRecordRepository.UpdateAsync(updatedMedicalRecord).Returns(Result<MedicalRecord>.Success(updatedMedicalRecord));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatedMedicalRecord, result.Data);
    }
}
