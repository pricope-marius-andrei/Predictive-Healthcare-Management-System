using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

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
		public async Task Handle_ReturnsMedicalHistories_WhenHistoriesExistForPatient()
		{
			// Arrange
			var query = new GetMedicalHistoriesByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
			var medicalHistories = new List<MedicalHistory>
			{
				new MedicalHistory
				{
					HistoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
					PatientId = query.PatientId,
					Condition = "Condition1",
					DateOfDiagnosis = DateTime.UtcNow
				},
				new MedicalHistory
				{
					HistoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
					PatientId = query.PatientId,
					Condition = "Condition2",
					DateOfDiagnosis = DateTime.UtcNow
				}
			};

			var medicalHistoryDtos = new List<MedicalHistoryDto>
			{
				new MedicalHistoryDto
				{
					HistoryId = medicalHistories[0].HistoryId,
					PatientId = medicalHistories[0].PatientId,
					Condition = medicalHistories[0].Condition,
					DateOfDiagnosis = medicalHistories[0].DateOfDiagnosis
				},
				new MedicalHistoryDto
				{
					HistoryId = medicalHistories[1].HistoryId,
					PatientId = medicalHistories[1].PatientId,
					Condition = medicalHistories[1].Condition,
					DateOfDiagnosis = medicalHistories[1].DateOfDiagnosis
				}
			};

			_mockMedicalHistoryRepository.GetByPatientIdAsync(query.PatientId).Returns(medicalHistories);
			_mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(medicalHistoryDtos, result.Data);
		}

		[Fact]
		public async Task Handle_ReturnsFailureResult_WhenNoHistoriesExistForPatient()
		{
			// Arrange
			var query = new GetMedicalHistoriesByPatientIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

			_mockMedicalHistoryRepository.GetByPatientIdAsync(query.PatientId).Returns(new List<MedicalHistory>());

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("No medical histories found for the specified patient.", result.ErrorMessage);
		}
	}
}