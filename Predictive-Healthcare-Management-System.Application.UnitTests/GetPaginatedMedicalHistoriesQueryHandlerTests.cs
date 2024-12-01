using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using Application.UseCases.QueryHandlers.MedicalHistory;
using AutoMapper;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetPaginatedMedicalHistoriesQueryHandlerTests
    {
        private readonly IMedicalHistoryRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly GetPaginatedMedicalHistoriesQueryHandler _handler;

        public GetPaginatedMedicalHistoriesQueryHandlerTests()
        {
            _repositoryMock = Substitute.For<IMedicalHistoryRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new GetPaginatedMedicalHistoriesQueryHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenMedicalHistoriesExist()
        {
            // Arrange
            var medicalHistory1Id = Guid.NewGuid();
            var medicalHistory2Id = Guid.NewGuid();
            var patient1Id = Guid.NewGuid();
            var patient2Id = Guid.NewGuid();

            var medicalHistories = new List<Domain.Entities.MedicalHistory>
            {
                new Domain.Entities.MedicalHistory
                {
                    HistoryId = medicalHistory1Id,
                    DateOfDiagnosis = DateTime.UtcNow.AddMonths(-6),
                    PatientId = patient1Id,
                    Condition = "Diabetes"
                },
                new Domain.Entities.MedicalHistory
                {
                    HistoryId = medicalHistory2Id,
                    DateOfDiagnosis = DateTime.UtcNow.AddMonths(-3),
                    PatientId = patient2Id,
                    Condition = "Hypertension"
                }
            };
            _repositoryMock.GetAllAsync().Returns(medicalHistories);

            var medicalHistoryDtos = medicalHistories.Select(m => new MedicalHistoryDto
            {
                HistoryId = m.HistoryId,
                DateOfDiagnosis = m.DateOfDiagnosis,
                PatientId = m.PatientId,
                Condition = m.Condition
            }).ToList();

            // Mock the mapper to handle any list of medical histories
            _mapperMock.Map<List<MedicalHistoryDto>>(Arg.Any<IEnumerable<Domain.Entities.MedicalHistory>>())
                .Returns(callInfo =>
                {
                    var inputHistories = callInfo.Arg<IEnumerable<Domain.Entities.MedicalHistory>>();
                    return inputHistories.Select(m => new MedicalHistoryDto
                    {
                        HistoryId = m.HistoryId,
                        DateOfDiagnosis = m.DateOfDiagnosis,
                        PatientId = m.PatientId,
                        Condition = m.Condition
                    }).ToList();
                });

            var query = new GetPaginatedMedicalHistoriesQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Data.Should().HaveCount(medicalHistories.Count);
            result.Data.TotalCount.Should().Be(medicalHistories.Count);
        }
    }
}
