﻿using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using Application.UseCases.QueryHandlers.MedicalRecord;
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
            var query = new GetMedicalRecordsByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = query.PatientId, DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), VisitReason = "Reason1", Symptoms = "Symptoms1", Diagnosis = "Diagnosis1", DoctorNotes = "Notes1", DateOfVisit = DateTime.UtcNow },
                new MedicalRecord { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), PatientId = query.PatientId, DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"), VisitReason = "Reason2", Symptoms = "Symptoms2", Diagnosis = "Diagnosis2", DoctorNotes = "Notes2", DateOfVisit = DateTime.UtcNow }
            };
            var medicalRecordDtos = new List<MedicalRecordDto>
            {
                new MedicalRecordDto { RecordId = medicalRecords[0].RecordId, PatientId = medicalRecords[0].PatientId, DoctorId = medicalRecords[0].DoctorId, VisitReason = "Reason1", Symptoms = "Symptoms1", Diagnosis = "Diagnosis1", DoctorNotes = "Notes1", DateOfVisit = DateTime.UtcNow },
                new MedicalRecordDto { RecordId = medicalRecords[1].RecordId, PatientId = medicalRecords[1].PatientId, DoctorId = medicalRecords[1].DoctorId, VisitReason = "Reason2", Symptoms = "Symptoms2", Diagnosis = "Diagnosis2", DoctorNotes = "Notes2", DateOfVisit = DateTime.UtcNow }
            };

            _mockMedicalRecordRepository.GetByPatientIdAsync(query.PatientId).Returns(Task.FromResult(Result<IEnumerable<MedicalRecord>>.Success(medicalRecords)));
            _mockMapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords).Returns(medicalRecordDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(medicalRecordDtos, result.Data);
        }
    }
}

