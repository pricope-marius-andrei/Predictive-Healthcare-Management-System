﻿using Application.DTOs;
using Application.UseCases.Queries.Patient;
using Application.UseCases.QueryHandlers.Patient;
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
            var query = new GetPatientByIdQuery { Id = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };
            var patient = new Patient
            {
                Id = query.Id,
                Username = "testuser",
                Email = "test@example.com",
                Password = "password",
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                // Address = "123 Test St",
                Gender = "Male",
                Height = 180,
                Weight = 75,
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateOfRegistration = DateTime.UtcNow
            };
            var patientDto = new PatientDto
            {
                PatientId = patient.Id,
                Username = patient.Username,
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                // Address = patient.Address,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                DateOfBirth = patient.DateOfBirth,
                DateOfRegistration = patient.DateOfRegistration
            };

            _mockPatientRepository.GetByIdAsync(query.Id).Returns(Task.FromResult(Result<Patient>.Success(patient)));
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
            var query = new GetPatientByIdQuery { Id = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf") };

            _mockPatientRepository.GetByIdAsync(query.Id).Returns(Task.FromResult(Result<Patient>.Failure("Patient not found")));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Patient not found", exception.Message);
        }
    }
}



