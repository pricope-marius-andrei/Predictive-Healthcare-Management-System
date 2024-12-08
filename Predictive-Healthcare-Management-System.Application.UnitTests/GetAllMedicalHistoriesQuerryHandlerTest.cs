﻿using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using Application.UseCases.QueryHandlers.MedicalHistory;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetAllMedicalHistoriesQueryHandlerTests
    {
        private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
        private readonly IMapper _mockMapper;
        private readonly GetAllMedicalHistoriesQueryHandler _handler;

        public GetAllMedicalHistoriesQueryHandlerTests()
        {
            _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _handler = new GetAllMedicalHistoriesQueryHandler(_mockMedicalHistoryRepository, _mockMapper);
        }

        [Fact]
        public async Task Handle_ReturnsAllMedicalHistories()
        {
            // Arrange
            var query = new GetAllMedicalHistoriesQuery();
            var medicalHistories = new List<MedicalHistory>
        {
            new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
            new MedicalHistory { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
        };
            var medicalHistoryDtos = new List<MedicalHistoryDto>
        {
            new MedicalHistoryDto { HistoryId = medicalHistories[0].HistoryId, PatientId = medicalHistories[0].PatientId, Condition = "Condition1", DateOfDiagnosis = DateTime.UtcNow },
            new MedicalHistoryDto { HistoryId = medicalHistories[1].HistoryId, PatientId = medicalHistories[1].PatientId, Condition = "Condition2", DateOfDiagnosis = DateTime.UtcNow }
        };

            _mockMedicalHistoryRepository.GetAllAsync().Returns(medicalHistories);
            _mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(medicalHistoryDtos, result);
        }
    }
}