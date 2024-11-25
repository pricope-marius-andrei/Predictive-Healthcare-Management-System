﻿using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using Application.UseCases.QueryHandlers.PatientQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
	public class GetAllPatientsQueryHandlerTests
	{
		private readonly IPatientRepository _mockPatientRepository;
		private readonly IMapper _mockMapper;
		private readonly GetAllPatientsQueryHandler _handler;

		public GetAllPatientsQueryHandlerTests()
		{
			_mockPatientRepository = Substitute.For<IPatientRepository>();
			_mockMapper = Substitute.For<IMapper>();
			_handler = new GetAllPatientsQueryHandler(_mockPatientRepository, _mockMapper);
		}

		[Fact]
		public async Task Handle_ReturnsPatientDtos_WhenPatientsExist()
		{
			// Arrange
			var query = new GetAllPatientsQuery();
			var patients = new List<Patient>
			{
				new Patient
				{
					PersonId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
					Username = "testuser1",
					Email = "test1@example.com",
					Password = "password",
					FirstName = "Test1",
					LastName = "User1",
					PhoneNumber = "1234567890",
					Address = "123 Test St",
					Gender = "Male",
					Height = 180,
					Weight = 75,
					DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
					DateOfRegistration = DateTime.UtcNow
				},
				new Patient
				{
					PersonId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
					Username = "testuser2",
					Email = "test2@example.com",
					Password = "password",
					FirstName = "Test2",
					LastName = "User2",
					PhoneNumber = "0987654321",
					Address = "456 Another St",
					Gender = "Female",
					Height = 165,
					Weight = 60,
					DateOfBirth = new DateTime(1990, 2, 2, 0, 0, 0, DateTimeKind.Utc),
					DateOfRegistration = DateTime.UtcNow
				}
			};

			var patientDtos = new List<PatientDto>
			{
				new PatientDto
				{
					PatientId = patients[0].PersonId,
					Username = patients[0].Username,
					Email = patients[0].Email,
					FirstName = patients[0].FirstName,
					LastName = patients[0].LastName,
					PhoneNumber = patients[0].PhoneNumber,
					Address = patients[0].Address,
					Gender = patients[0].Gender,
					Height = patients[0].Height,
					Weight = patients[0].Weight,
					DateOfBirth = patients[0].DateOfBirth,
					DateOfRegistration = patients[0].DateOfRegistration
				},
				new PatientDto
				{
					PatientId = patients[1].PersonId,
					Username = patients[1].Username,
					Email = patients[1].Email,
					FirstName = patients[1].FirstName,
					LastName = patients[1].LastName,
					PhoneNumber = patients[1].PhoneNumber,
					Address = patients[1].Address,
					Gender = patients[1].Gender,
					Height = patients[1].Height,
					Weight = patients[1].Weight,
					DateOfBirth = patients[1].DateOfBirth,
					DateOfRegistration = patients[1].DateOfRegistration
				}
			};

			_mockPatientRepository.GetAllAsync().Returns(patients);
			_mockMapper.Map<IEnumerable<PatientDto>>(patients).Returns(patientDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(patientDtos, result.Data);
		}

		[Fact]
		public async Task Handle_ReturnsEmptyResult_WhenNoPatientsExist()
		{
			// Arrange
			var query = new GetAllPatientsQuery();
			var patients = new List<Patient>();
			var patientDtos = new List<PatientDto>();

			_mockPatientRepository.GetAllAsync().Returns(patients);
			_mockMapper.Map<IEnumerable<PatientDto>>(patients).Returns(patientDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Empty(result.Data);
		}
	}
}