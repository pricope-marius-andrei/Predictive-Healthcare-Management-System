using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using Application.UseCases.QueryHandlers.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using System.Collections.Generic; // Add this using directive
using System.Threading;
using System.Threading.Tasks; // Add this using directive

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
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
            var query = new GetMedicalHistoriesByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var medicalHistories = new List<MedicalHistory>
            {
                new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = query.PatientId, Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
                new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = query.PatientId, Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
            };
            var medicalHistoryDtos = new List<MedicalHistoryDto>
            {
                new MedicalHistoryDto { HistoryId = medicalHistories[0].HistoryId, PatientId = medicalHistories[0].PatientId, Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
                new MedicalHistoryDto { HistoryId = medicalHistories[1].HistoryId, PatientId = medicalHistories[1].PatientId, Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
            };

            _mockMedicalHistoryRepository.GetByPatientIdAsync(query.PatientId).Returns(Task.FromResult(Result<IEnumerable<MedicalHistory>>.Success(medicalHistories)));
            _mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(medicalHistoryDtos, result.Data);
        }
    }
}
