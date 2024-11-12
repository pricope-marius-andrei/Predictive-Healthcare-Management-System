using Application.UseCases.QueryHandlers;
using Application.UseCases.Queries;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

public class GetMedicalRecordsByPatientIdQueryHandlerTests
{
    private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
    private readonly IMapper _mockMapper;
    private readonly GetMedicalRecordsByPatientIdQueryHandler _handler;

    public GetMedicalRecordsByPatientIdQueryHandlerTests()
    {
        _mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new GetMedicalRecordsByPatientIdQueryHandler(_mockMedicalRecordRepository, _mockMapper);
    }

    [Fact]
    public async Task Handle_ReturnsMedicalRecordsByPatientId()
    {
        // Arrange
        var query = new GetMedicalRecordsByPatientIdQuery { PatientId = Guid.NewGuid() };
        var medicalRecords = new List<MedicalRecord>
        {
            new MedicalRecord { RecordId = Guid.NewGuid(), PatientId = query.PatientId, DoctorId = Guid.NewGuid(), VisitReason = "Reason1", Symptoms = "Symptoms1", Diagnosis = "Diagnosis1", DoctorNotes = "Notes1", DateOfVisit = DateTime.UtcNow },
            new MedicalRecord { RecordId = Guid.NewGuid(), PatientId = query.PatientId, DoctorId = Guid.NewGuid(), VisitReason = "Reason2", Symptoms = "Symptoms2", Diagnosis = "Diagnosis2", DoctorNotes = "Notes2", DateOfVisit = DateTime.UtcNow }
        };
        var medicalRecordDtos = new List<MedicalRecordDto>
        {
            new MedicalRecordDto { RecordId = medicalRecords[0].RecordId, PatientId = medicalRecords[0].PatientId, DoctorId = medicalRecords[0].DoctorId, VisitReason = "Reason1", Symptoms = "Symptoms1", Diagnosis = "Diagnosis1", DoctorNotes = "Notes1", DateOfVisit = DateTime.UtcNow },
            new MedicalRecordDto { RecordId = medicalRecords[1].RecordId, PatientId = medicalRecords[1].PatientId, DoctorId = medicalRecords[1].DoctorId, VisitReason = "Reason2", Symptoms = "Symptoms2", Diagnosis = "Diagnosis2", DoctorNotes = "Notes2", DateOfVisit = DateTime.UtcNow }
        };

        _mockMedicalRecordRepository.GetByPatientIdAsync(query.PatientId).Returns(medicalRecords);
        _mockMapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords).Returns(medicalRecordDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(medicalRecordDtos, result);
    }
}
