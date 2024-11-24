using Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers;
using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
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
		public async Task Handle_ShouldReturnFailure_WhenMedicalRecordNotFound()
		{
			// Arrange
			var command = new UpdateMedicalRecordCommand { RecordId = Guid.NewGuid() };
			_mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns((MedicalRecord?)null);

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Medical record not found.", result.ErrorMessage);
		}

		[Fact]
		public async Task Handle_ShouldUpdateMedicalRecord_WhenRequestIsValid()
		{
			// Arrange
			var command = new UpdateMedicalRecordCommand
			{
				RecordId = Guid.NewGuid(),
				VisitReason = "Updated Visit Reason",
				Symptoms = "Updated Symptoms",
				Diagnosis = "Updated Diagnosis",
				DoctorNotes = "Updated Doctor Notes",
				DateOfVisit = DateTime.UtcNow
			};

			var existingMedicalRecord = new MedicalRecord
			{
				RecordId = command.RecordId,
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Initial Visit Reason",
				Symptoms = "Initial Symptoms",
				Diagnosis = "Initial Diagnosis",
				DoctorNotes = "Initial Doctor Notes",
				DateOfVisit = DateTime.UtcNow.AddDays(-7)
			};

			_mockMedicalRecordRepository.GetByIdAsync(command.RecordId).Returns(existingMedicalRecord);

			// Corrected AutoMapper mock setup
			_mockMapper.When(m => m.Map(command, existingMedicalRecord)).Do(_ =>
			{
				existingMedicalRecord.VisitReason = command.VisitReason;
				existingMedicalRecord.Symptoms = command.Symptoms;
				existingMedicalRecord.Diagnosis = command.Diagnosis;
				existingMedicalRecord.DoctorNotes = command.DoctorNotes;
				existingMedicalRecord.DateOfVisit = command.DateOfVisit;
			});

			// Corrected repository mock setup
			_mockMedicalRecordRepository.UpdateAsync(existingMedicalRecord).Returns(Result<MedicalRecord>.Success(existingMedicalRecord));

			// Act
			var result = await _handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Data);
			Assert.Equal(existingMedicalRecord.VisitReason, result.Data.VisitReason);
			Assert.Equal(existingMedicalRecord.Symptoms, result.Data.Symptoms);
			Assert.Equal(existingMedicalRecord.Diagnosis, result.Data.Diagnosis);
			Assert.Equal(existingMedicalRecord.DoctorNotes, result.Data.DoctorNotes);
			Assert.Equal(existingMedicalRecord.DateOfVisit, result.Data.DateOfVisit);
		}
	}
}