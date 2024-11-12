using Application.UseCases.QueryHandlers;
using Application.UseCases.Queries;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

public class GetMedicalHistoryByIdQueryHandlerTests
{
    private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
    private readonly IMapper _mockMapper;
    private readonly GetMedicalHistoryByIdQueryHandler _handler;

    public GetMedicalHistoryByIdQueryHandlerTests()
    {
        _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
        _mockMapper = Substitute.For<IMapper>();
        _handler = new GetMedicalHistoryByIdQueryHandler(_mockMedicalHistoryRepository, _mockMapper);
    }

    [Fact]
    public async Task Handle_ReturnsMedicalHistoryById()
    {
        // Arrange
        var query = new GetMedicalHistoryByIdQuery { HistoryId = Guid.NewGuid() };
        var medicalHistory = new MedicalHistory
        {
            HistoryId = query.HistoryId,
            PatientId = Guid.NewGuid(),
            Condition = "Condition1",
            DateOfDiagnosis = DateTime.UtcNow
        };
        var medicalHistoryDto = new MedicalHistoryDto
        {
            HistoryId = medicalHistory.HistoryId,
            PatientId = medicalHistory.PatientId,
            Condition = "Condition1",
            DateOfDiagnosis = DateTime.UtcNow
        };

        _mockMedicalHistoryRepository.GetByIdAsync(query.HistoryId).Returns(medicalHistory);
        _mockMapper.Map<MedicalHistoryDto>(medicalHistory).Returns(medicalHistoryDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(medicalHistoryDto, result);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenMedicalHistoryNotFound()
    {
        // Arrange
        var query = new GetMedicalHistoryByIdQuery { HistoryId = Guid.NewGuid() };

        _mockMedicalHistoryRepository.GetByIdAsync(query.HistoryId).Returns((MedicalHistory)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
