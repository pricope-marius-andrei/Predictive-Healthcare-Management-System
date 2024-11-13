using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
    public class GetAllDoctorQueryHandlerTests
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly GetAllDoctorQueryHandler _handler;

        public GetAllDoctorQueryHandlerTests()
        {
            _repository = Substitute.For<IDoctorRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllDoctorQueryHandler(_repository, _mapper);
        }

        [Fact]
        public async Task Handle_ReturnsMappedDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new()
                {
                    DoctorId = Guid.NewGuid(),
                    Username = "doc1",
                    Email = "doc1@example.com",
                    Password = "password",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    Specialization = "Cardiology",
                    MedicalRecords = new List<MedicalRecord>()
                }
            };
            var doctorDtos = new List<DoctorDto>
            {
                new()
                {
                    DoctorId = doctors[0].DoctorId,
                    Username = "doc1",
                    Email = "doc1@example.com"
                }
            };

            _repository.GetAllAsync().Returns(doctors);
            _mapper.Map<IEnumerable<DoctorDto>>(doctors).Returns(doctorDtos);

            var query = new GetAllDoctorsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(doctorDtos, result);
        }
    }
}
