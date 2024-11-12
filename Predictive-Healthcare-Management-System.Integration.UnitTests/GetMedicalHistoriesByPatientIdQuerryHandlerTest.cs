using Application.UseCases.QueryHandlers;
using Application.UseCases.Queries;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

public class GetMedicalHistoriesByPatientIdQueryHandlerTests
{
    private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
    private readonly IMapper _mockMapper;
    private readonly GetMedicalHistoriesByPatientIdQueryHandler _handler;

    public GetMedicalHistoriesByPatientIdQueryHandlerTests()
    {
        _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new GetMedicalHistoriesByPatientIdQueryHandler(_mockMedicalHistoryRepository, _mockMapper);
    }

    [Fact]
    public async Task Handle_ReturnsMedicalHistoriesByPatientId()
    {
        // Arrange
        var query = new GetMedicalHistoriesByPatientIdQuery { PatientId = Guid.NewGuid() };
        var medicalHistories = new List<MedicalHistory>
        {
            new MedicalHistory { HistoryId = Guid.NewGuid(), PatientId = query.PatientId, Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
            new MedicalHistory { HistoryId = Guid.NewGuid(), PatientId = query.PatientId, Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
        };
        var medicalHistoryDtos = new List<MedicalHistoryDto>
        {
            new MedicalHistoryDto { HistoryId = medicalHistories[0].HistoryId, PatientId = medicalHistories[0].PatientId, Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
            new MedicalHistoryDto { HistoryId = medicalHistories[1].HistoryId, PatientId = medicalHistories[1].PatientId, Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
        };

        _mockMedicalHistoryRepository.GetByPatientIdAsync(query.PatientId).Returns(medicalHistories);
        _mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(medicalHistoryDtos, result);
    }
}
