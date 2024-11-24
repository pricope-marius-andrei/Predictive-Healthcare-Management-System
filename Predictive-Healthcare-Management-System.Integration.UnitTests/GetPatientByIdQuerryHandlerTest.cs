using Application.DTOs;
using Application.UseCases.Queries.PatientQueries;
using Application.UseCases.QueryHandlers.PatientQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class GetPatientByIdQueryHandlerTests
    {
        private readonly IPatientRepository _mockPatientRepository;
        private readonly IMapper _mockMapper;
        private readonly GetPatientByIdQueryHandler _handler;

        public GetPatientByIdQueryHandlerTests()
        {
            _mockPatientRepository = Substitute.For<IPatientRepository>();
            _mockMapper = Substitute.For<IMapper>();
            _handler = new GetPatientByIdQueryHandler(_mockPatientRepository, _mockMapper);
        }

        [Fact]
        public async Task Handle_ReturnsPatientDto_WhenPatientExists()
        {
            // Arrange
            var query = new GetPatientByIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var patient = new Patient
            {
                PersonId = query.PatientId,
                Username = "testuser",
                Email = "test@example.com",
                Password = "password",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                Address = "123 Test St",
                Gender = "Male",
                Height = 180,
                Weight = 75,
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateOfRegistration = DateTime.UtcNow
            };
            var patientDto = new PatientDto
            {
                PatientId = patient.PersonId,
                Username = patient.Username,
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                DateOfBirth = patient.DateOfBirth,
                DateOfRegistration = patient.DateOfRegistration
            };

            _mockPatientRepository.GetByIdAsync(query.PatientId).Returns(patient);
            _mockMapper.Map<PatientDto>(patient).Returns(patientDto);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(patientDto, response.Data);
		}

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenPatientDoesNotExist()
        {
	        // Arrange
	        var query = new GetPatientByIdQuery { PatientId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

	        _mockPatientRepository.GetByIdAsync(query.PatientId).Returns((Patient?)null);

	        // Act & Assert
	        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
	        Assert.Equal($"Patient with ID {query.PatientId} was not found.", exception.Message);
        }
	}
}