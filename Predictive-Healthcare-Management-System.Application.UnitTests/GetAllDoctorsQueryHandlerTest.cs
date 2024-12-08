using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using Application.UseCases.QueryHandlers.Doctor;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetAllDoctorsQueryHandlerTest
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDoctorsQueryHandlerTest()
        {
            _repository = Substitute.For<IDoctorRepository>();
            _mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Given_GetAllDoctorsQueryHandler_When_Invoked_Then_AListOfDoctorsShouldBeReturned()
        {
            // Arrange
            List<Doctor> doctors = GenerateDoctors();
            _repository.GetAllAsync().Returns(doctors);
            var query = new GetAllDoctorsQuery();
            _mapper.Map<IEnumerable<DoctorDto>>(doctors).Returns(new List<DoctorDto>
                {
                    new DoctorDto
                    {
                        DoctorId = doctors[0].Id,
                        Username = doctors[0].Username,
                        FirstName = doctors[0].FirstName,
                        LastName = doctors[0].LastName,
                        Email = doctors[0].Email,
                        Specialization = doctors[0].Specialization,
                        PhoneNumber = doctors[0].PhoneNumber,
                        DateOfRegistration = doctors[0].DateOfRegistration
                    },
                    new DoctorDto
                    {
                        DoctorId = doctors[1].Id,
                        Username = doctors[1].Username,
                        FirstName = doctors[1].FirstName,
                        LastName = doctors[1].LastName,
                        Email = doctors[1].Email,
                        Specialization = doctors[1].Specialization,
                        PhoneNumber = doctors[1].PhoneNumber,
                        DateOfRegistration = doctors[1].DateOfRegistration
                    }
                });
            var handler = new GetAllDoctorQueryHandler(_repository, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(doctors[0].Id, result.First().DoctorId);
            Assert.Equal(doctors[0].FirstName, result.First().FirstName);
            Assert.Equal(doctors[0].Username, result.First().Username);
            Assert.Equal(doctors[0].LastName, result.First().LastName);
            Assert.Equal(doctors[0].Email, result.First().Email);
            Assert.Equal(doctors[0].Specialization, result.First().Specialization);
            Assert.Equal(doctors[0].PhoneNumber, result.First().PhoneNumber);
            Assert.Equal(doctors[0].DateOfRegistration, result.First().DateOfRegistration);

            Assert.Equal(doctors[1].Id, result.Last().DoctorId);
            Assert.Equal(doctors[1].FirstName, result.Last().FirstName);
            Assert.Equal(doctors[1].Username, result.Last().Username);
            Assert.Equal(doctors[1].LastName, result.Last().LastName);
            Assert.Equal(doctors[1].Email, result.Last().Email);
            Assert.Equal(doctors[1].Specialization, result.Last().Specialization);
            Assert.Equal(doctors[1].PhoneNumber, result.Last().PhoneNumber);
            Assert.Equal(doctors[1].DateOfRegistration, result.Last().DateOfRegistration);
        }

        private static List<Doctor> GenerateDoctors()
        {
            return new List<Doctor>
                {
                    new Doctor
                    {
                        Id = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                        Username = "johndoe123",
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "johndoe123@gmail.com",
                        Password = "password123",
                        Specialization = "Cardiology",
                        PhoneNumber = "1234567890",
                        DateOfRegistration = DateTime.Now,
                        MedicalRecords = new List<MedicalRecord>()
                    },
                    new Doctor
                    {
                        Id = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf"),
                        Username = "janedoe321",
                        FirstName = "Jane",
                        LastName = "Doe",
                        Email = "janedoe321@yahoo.com",
                        Password = "password321",
                        Specialization = "Neurology",
                        PhoneNumber = "0987654321",
                        DateOfRegistration = DateTime.Now,
                        MedicalRecords = new List<MedicalRecord>()
                    }
                };
        }
    }
}
