﻿using Application.DTOs;
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
            var query = new GetMedicalHistoryByIdQuery { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var medicalHistory = new MedicalHistory
            {
                HistoryId = query.HistoryId,
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
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

            _mockMedicalHistoryRepository.GetByIdAsync(query.HistoryId).Returns(Task.FromResult(Result<MedicalHistory>.Success(medicalHistory)));
            _mockMapper.Map<MedicalHistoryDto>(medicalHistory).Returns(medicalHistoryDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(medicalHistoryDto, result.Data);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenMedicalHistoryNotFound()
        {
            // Arrange
            var query = new GetMedicalHistoryByIdQuery { HistoryId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

            _mockMedicalHistoryRepository.GetByIdAsync(query.HistoryId).Returns(Task.FromResult(Result<MedicalHistory>.Failure("Medical history not found")));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Medical history not found", exception.Message);
        }
    }
}

