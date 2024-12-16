using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using Application.UseCases.QueryHandlers.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetPaginatedDoctorsQueryHandlerTests
    {
        private readonly IDoctorRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly GetPaginatedDoctorsQueryHandler _handler;

        public GetPaginatedDoctorsQueryHandlerTests()
        {
            _repositoryMock = Substitute.For<IDoctorRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new GetPaginatedDoctorsQueryHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenDoctorsExist()
        {
            // Arrange
            var doctors = new List<Domain.Entities.Doctor>
            {
                new Domain.Entities.Doctor
                {
                    Id = Guid.NewGuid(),
                    Username = "doctor1",
                    Email = "doctor1@example.com",
                    Password = "Password123!",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    Specialization = "Cardiology",
                    MedicalRecords = null
                },
                new Domain.Entities.Doctor
                {
                    Id = Guid.NewGuid(),
                    Username = "doctor2",
                    Email = "doctor2@example.com",
                    Password = "SecurePass456!",
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = "9876543210",
                    Specialization = "Dermatology",
                    MedicalRecords = null
                }
            };
            var returnedResult = _repositoryMock.GetAllAsync();

            returnedResult.Returns(Result<IEnumerable<Domain.Entities.Doctor>>.Success(doctors));

            var doctorDtos = doctors.Select(d => new DoctorDto
            {
                DoctorId = d.Id,
                Username = d.Username,
                Email = d.Email,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                PhoneNumber = d.PhoneNumber,
                DateOfRegistration = d.DateOfRegistration,
                MedicalRecords = _mapperMock.Map<List<MedicalRecordDto>>(d.MedicalRecords)
            }).ToList();
            _mapperMock.Map<List<DoctorDto>>(Arg.Any<IEnumerable<Domain.Entities.Doctor>>()).Returns(doctorDtos);

            var query = new GetPaginatedDoctorsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(doctorDtos.Count, result.Data.Data.Count);
            Assert.Equal(doctors.Count, result.Data.TotalCount);
        }
    }
}
