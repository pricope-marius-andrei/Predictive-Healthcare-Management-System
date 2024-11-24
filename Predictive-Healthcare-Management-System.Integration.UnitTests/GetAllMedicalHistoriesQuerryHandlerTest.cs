using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
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
		public async Task Handle_ShouldReturnAllMedicalHistories_WhenHistoriesExist()
		{
			// Arrange
			var query = new GetAllMedicalHistoriesQuery();
			var medicalHistories = new List<MedicalHistory>
			{
				new MedicalHistory
				{
					HistoryId = Guid.NewGuid(),
					PatientId = Guid.NewGuid(),
					Condition = "Condition1",
					DateOfDiagnosis = DateTime.UtcNow
				},
				new MedicalHistory
				{
					HistoryId = Guid.NewGuid(),
					PatientId = Guid.NewGuid(),
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

			_mockMedicalHistoryRepository.GetAllAsync().Returns(medicalHistories);
			_mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Data);
			Assert.Equal(medicalHistoryDtos, result.Data);
		}

		[Fact]
		public async Task Handle_ShouldReturnEmptyResult_WhenNoHistoriesExist()
		{
			// Arrange
			var query = new GetAllMedicalHistoriesQuery();
			var medicalHistories = new List<MedicalHistory>(); // Empty list
			var medicalHistoryDtos = new List<MedicalHistoryDto>(); // Empty list

			_mockMedicalHistoryRepository.GetAllAsync().Returns(medicalHistories);
			_mockMapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories).Returns(medicalHistoryDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess); // Now expecting a successful result
			Assert.NotNull(result.Data);
			Assert.Empty(result.Data); // Data should be an empty collection
		}
	}
}