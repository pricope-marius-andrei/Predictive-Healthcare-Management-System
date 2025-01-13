using Application.UseCases.CommandHandlers.MedicalRecord;
using Application.UseCases.Commands.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using System.Collections.Generic; // Add this using directive
using System.Threading;
using System.Threading.Tasks; // Add this using directive
using Xunit;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
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
            var command = new UpdateMedicalRecordCommand { RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(Task.FromResult(Result<MedicalRecord>.Failure("Medical record not found")));

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
                RecordId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                VisitReason = "Updated Visit Reason",
                Symptoms = "Updated Symptoms",
                Diagnosis = "Updated Diagnosis",
                DoctorNotes = "Updated Doctor Notes",
                DateOfVisit = DateTime.UtcNow
            };

            var existingMedicalRecord = new MedicalRecord
            {
                RecordId = command.RecordId,
                PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                DoctorId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
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

            _mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(Task.FromResult(Result<MedicalRecord>.Success(existingMedicalRecord)));
            _mockMapper.Map(command, existingMedicalRecord).Returns(updatedMedicalRecord);
            _mockMedicalRecordRepository.UpdateAsync(updatedMedicalRecord).Returns(Task.FromResult(Result<MedicalRecord>.Success(updatedMedicalRecord)));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updatedMedicalRecord, result.Data);
        }
    }
}




