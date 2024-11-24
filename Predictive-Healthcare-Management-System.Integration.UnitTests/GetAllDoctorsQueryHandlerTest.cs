using Application.DTOs;
using Application.UseCases.Queries.DoctorQueries;
using Application.UseCases.QueryHandlers.DoctorQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
	public class GetAllDoctorsQueryHandlerTest
	{
		private readonly IDoctorRepository _mockRepository;
		private readonly IMapper _mockMapper;
		private readonly GetAllDoctorsQueryHandler _handler;

		public GetAllDoctorsQueryHandlerTest()
		{
			_mockRepository = Substitute.For<IDoctorRepository>();
			_mockMapper = Substitute.For<IMapper>();
			_handler = new GetAllDoctorsQueryHandler(_mockRepository, _mockMapper);
		}

		[Fact]
		public async Task Handle_ReturnsAllDoctors_WhenDoctorsExist()
		{
			// Arrange
			var query = new GetAllDoctorsQuery();
			var doctors = GenerateDoctors();

			var fixedDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			var doctorDtos = doctors.Select(doctor => new DoctorDto
			{
				DoctorId = doctor.PersonId, // Map PersonId to DoctorId
				Username = doctor.Username,
				FirstName = doctor.FirstName,
				LastName = doctor.LastName,
				Email = doctor.Email,
				Specialization = doctor.Specialization,
				PhoneNumber = doctor.PhoneNumber,
				DateOfRegistration = fixedDate // Ensure consistent DateTime
			}).ToList();

			_mockRepository.GetAllAsync().Returns(doctors);
			_mockMapper.Map<IEnumerable<DoctorDto>>(doctors).Returns(doctorDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Data);
			Assert.Equal(doctorDtos.Count, result.Data.Count());

			Assert.Collection(result.Data,
				doctorDto =>
				{
					var expected = doctorDtos[0];
					Assert.Equal(expected.DoctorId, doctorDto.DoctorId);
					Assert.Equal(expected.Username, doctorDto.Username);
					Assert.Equal(expected.FirstName, doctorDto.FirstName);
					Assert.Equal(expected.LastName, doctorDto.LastName);
					Assert.Equal(expected.Email, doctorDto.Email);
					Assert.Equal(expected.Specialization, doctorDto.Specialization);
					Assert.Equal(expected.PhoneNumber, doctorDto.PhoneNumber);
					Assert.Equal(expected.DateOfRegistration, doctorDto.DateOfRegistration);
				},
				doctorDto =>
				{
					var expected = doctorDtos[1];
					Assert.Equal(expected.DoctorId, doctorDto.DoctorId);
					Assert.Equal(expected.Username, doctorDto.Username);
					Assert.Equal(expected.FirstName, doctorDto.FirstName);
					Assert.Equal(expected.LastName, doctorDto.LastName);
					Assert.Equal(expected.Email, doctorDto.Email);
					Assert.Equal(expected.Specialization, doctorDto.Specialization);
					Assert.Equal(expected.PhoneNumber, doctorDto.PhoneNumber);
					Assert.Equal(expected.DateOfRegistration, doctorDto.DateOfRegistration);
				});

			// Verify that the repository and mapper were called as expected
			await _mockRepository.Received(1).GetAllAsync();
			_mockMapper.Received(1).Map<IEnumerable<DoctorDto>>(doctors);
		}

		[Fact]
		public async Task Handle_ReturnsEmptyResult_WhenNoDoctorsExist()
		{
			// Arrange
			var query = new GetAllDoctorsQuery();
			var doctors = new List<Doctor>(); // Empty list
			var doctorDtos = new List<DoctorDto>(); // Empty list

			_mockRepository.GetAllAsync().Returns(doctors);
			_mockMapper.Map<IEnumerable<DoctorDto>>(doctors).Returns(doctorDtos);

			// Act
			var result = await _handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Data);
			Assert.Empty(result.Data);
		}

		private static List<Doctor> GenerateDoctors()
		{
			var fixedDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return new List<Doctor>
			{
				new Doctor
				{
					PersonId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
					Username = "johndoe123",
					FirstName = "John",
					LastName = "Doe",
					Email = "johndoe123@gmail.com",
					Password = "password123",
					Specialization = "Cardiology",
					PhoneNumber = "1234567890",
					DateOfRegistration = fixedDate
				},
				new Doctor
				{
					PersonId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
					Username = "janedoe321",
					FirstName = "Jane",
					LastName = "Doe",
					Email = "janedoe321@yahoo.com",
					Password = "password321",
					Specialization = "Neurology",
					PhoneNumber = "0987654321",
					DateOfRegistration = fixedDate
				}
			};
		}
	}
}