using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
	public class GetMedicalRecordByIdQueryHandlerTests
	{
		private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
		private readonly IMapper _mockMapper;
		private readonly GetMedicalRecordByIdQueryHandler _handler;

		public GetMedicalRecordByIdQueryHandlerTests()
		{
			_mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
			_mockMapper = Substitute.For<IMapper>();
			_handler = new GetMedicalRecordByIdQueryHandler(_mockMedicalRecordRepository, _mockMapper);
		}

		[Fact]
		public async Task Handle_ShouldReturnMedicalRecordById_WhenRecordExists()
		{
			// Arrange
			var query = new GetMedicalRecordByIdQuery { RecordId = Guid.NewGuid() };
			var medicalRecord = new MedicalRecord
			{
				RecordId = query.RecordId,
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Routine Checkup",
				Symptoms = "None",
				Diagnosis = "Healthy",
				DoctorNotes = "No issues found",
				DateOfVisit = DateTime.UtcNow
			};

			var medicalRecordDto = new MedicalRecordDto
			{
				RecordId = medicalRecord.RecordId,
				PatientId = medicalRecord.PatientId,
				DoctorId = medicalRecord.DoctorId,
				VisitReason = medicalRecord.VisitReason,
				Symptoms = medicalRecord.Symptoms,
				Diagnosis = medicalRecord.Diagnosis,
				DoctorNotes = medicalRecord.DoctorNotes,
				DateOfVisit = medicalRecord.DateOfVisit
			};

			_mockMedicalRecordRepository.GetByIdAsync(query.RecordId).Returns(medicalRecord);
			_mockMapper.Map<MedicalRecordDto>(medicalRecord).Returns(medicalRecordDto);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(medicalRecordDto, result.Data);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenMedicalRecordNotFound()
		{
			// Arrange
			var query = new GetMedicalRecordByIdQuery { RecordId = Guid.NewGuid() };

			_mockMedicalRecordRepository.GetByIdAsync(query.RecordId).Returns((MedicalRecord?)null);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Medical record not found.", result.ErrorMessage);
		}
	}
}