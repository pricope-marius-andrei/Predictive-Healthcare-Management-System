using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

public class GetMedicalRecordByIdQueryHandlerTests
{
    private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
    private readonly IMapper _mockMapper;
    private readonly GetMedicalRecordByIdQueryHandler _handler;

    public GetMedicalRecordByIdQueryHandlerTests()
    {
        _mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new GetMedicalRecordByIdQueryHandler(_mockMedicalRecordRepository, _mockMapper);
    }

    [Fact]
    public async Task Handle_ReturnsMedicalRecordById()
    {
        // Arrange
        var query = new GetMedicalRecordByIdQuery { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
        var medicalRecord = new MedicalRecord
        {
            RecordId = query.RecordId,
            PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
            DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
            VisitReason = "Reason1",
            Symptoms = "Symptoms1",
            Diagnosis = "Diagnosis1",
            DoctorNotes = "Notes1",
            DateOfVisit = DateTime.UtcNow
        };
        var medicalRecordDto = new MedicalRecordDto
        {
            RecordId = medicalRecord.RecordId,
            PatientId = medicalRecord.PatientId,
            DoctorId = medicalRecord.DoctorId,
            VisitReason = "Reason1",
            Symptoms = "Symptoms1",
            Diagnosis = "Diagnosis1",
            DoctorNotes = "Notes1",
            DateOfVisit = DateTime.UtcNow
        };

        _mockMedicalRecordRepository.GetByIdAsync(query.RecordId).Returns(medicalRecord);
        _mockMapper.Map<MedicalRecordDto>(medicalRecord).Returns(medicalRecordDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(medicalRecordDto, result);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenMedicalRecordNotFound()
    {
        // Arrange
        var query = new GetMedicalRecordByIdQuery { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

        _mockMedicalRecordRepository.GetByIdAsync(query.RecordId).Returns((MedicalRecord)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
