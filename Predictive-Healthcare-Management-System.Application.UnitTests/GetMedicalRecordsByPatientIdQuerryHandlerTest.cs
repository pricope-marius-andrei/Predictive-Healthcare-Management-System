using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

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
		public async Task Handle_ReturnsMedicalRecords_WhenRecordsExistForPatient()
		{
			// Arrange
			var query = new GetMedicalRecordsByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
			var medicalRecords = new List<MedicalRecord>
			{
				new MedicalRecord
				{
					RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
					PatientId = query.PatientId,
					DoctorId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
					VisitReason = "Reason1",
					Symptoms = "Symptoms1",
					Diagnosis = "Diagnosis1",
					DoctorNotes = "Notes1",
					DateOfVisit = DateTime.UtcNow
				},
				new MedicalRecord
				{
					RecordId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
					PatientId = query.PatientId,
					DoctorId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
					VisitReason = "Reason2",
					Symptoms = "Symptoms2",
					Diagnosis = "Diagnosis2",
					DoctorNotes = "Notes2",
					DateOfVisit = DateTime.UtcNow
				}
			};

			var medicalRecordDtos = new List<MedicalRecordDto>
			{
				new MedicalRecordDto
				{
					RecordId = medicalRecords[0].RecordId,
					PatientId = medicalRecords[0].PatientId,
					DoctorId = medicalRecords[0].DoctorId,
					VisitReason = medicalRecords[0].VisitReason,
					Symptoms = medicalRecords[0].Symptoms,
					Diagnosis = medicalRecords[0].Diagnosis,
					DoctorNotes = medicalRecords[0].DoctorNotes,
					DateOfVisit = medicalRecords[0].DateOfVisit
				},
				new MedicalRecordDto
				{
					RecordId = medicalRecords[1].RecordId,
					PatientId = medicalRecords[1].PatientId,
					DoctorId = medicalRecords[1].DoctorId,
					VisitReason = medicalRecords[1].VisitReason,
					Symptoms = medicalRecords[1].Symptoms,
					Diagnosis = medicalRecords[1].Diagnosis,
					DoctorNotes = medicalRecords[1].DoctorNotes,
					DateOfVisit = medicalRecords[1].DateOfVisit
				}
			};

			_mockMedicalRecordRepository.GetByPatientIdAsync(query.PatientId).Returns(medicalRecords);
			_mockMapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords).Returns(medicalRecordDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(medicalRecordDtos, result.Data);
		}

		[Fact]
		public async Task Handle_ReturnsFailureResult_WhenNoRecordsExistForPatient()
		{
			// Arrange
			var query = new GetMedicalRecordsByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

			_mockMedicalRecordRepository.GetByPatientIdAsync(query.PatientId).Returns(new List<MedicalRecord>());

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("No medical records found for the specified patient.", result.ErrorMessage);
		}
	}
}