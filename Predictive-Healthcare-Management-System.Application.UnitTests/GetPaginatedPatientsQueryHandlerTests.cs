using Application.DTOs;
using Application.UseCases.Queries.Patient;
using Application.UseCases.QueryHandlers.Patient;
using AutoMapper;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetPaginatedPatientsQueryHandlerTests
    {
        private readonly IPatientRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly GetPaginatedPatientsQueryHandler _handler;

        public GetPaginatedPatientsQueryHandlerTests()
        {
            _repositoryMock = Substitute.For<IPatientRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new GetPaginatedPatientsQueryHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenPatientsExist()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var medicalHistoryId = Guid.NewGuid();
            var medicalRecordId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();

            var patients = new List<Domain.Entities.Patient>
            {
                new Domain.Entities.Patient
                {
                    Id = patientId,
                    Username = "patient1",
                    Email = "patient1@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "1234567890",
                    Gender = "Male",
                    Height = 180,
                    Weight = 75,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DateOfRegistration = DateTime.UtcNow,
                    MedicalHistories = new List<Domain.Entities.MedicalHistory>
                    {
                        new Domain.Entities.MedicalHistory
                        {
                            HistoryId = medicalHistoryId,
                            DateOfDiagnosis = DateTime.UtcNow,
                            Condition = "Diabetes",
                            PatientId = patientId
                        }
                    },
                    MedicalRecords = new List<Domain.Entities.MedicalRecord>
                    {
                        new Domain.Entities.MedicalRecord
                        {
                            RecordId = medicalRecordId,
                            VisitReason = "Checkup",
                            Symptoms = "None",
                            Diagnosis = "Healthy",
                            DoctorNotes = "Routine checkup",
                            DoctorId = doctorId,
                            PatientId = patientId,
                            DateOfVisit = DateTime.UtcNow
                        }
                    }
                }
            };

            // Mock repository to return the list of patients
            _repositoryMock.GetAllAsync()
                .Returns(Task.FromResult(Result<IEnumerable<Domain.Entities.Patient>>.Success(patients)));

            // Mock repository to return the total count of patients
            _repositoryMock.CountAsync(Arg.Any<IEnumerable<Domain.Entities.Patient>>())
                .Returns(Task.FromResult(Result<int>.Success(patients.Count)));

            // Mock repository to return the paginated list of patients
            _repositoryMock.GetPaginatedAsync(Arg.Any<IEnumerable<Domain.Entities.Patient>>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Task.FromResult(Result<List<Domain.Entities.Patient>>.Success(patients)));

            // Prepare the expected DTOs
            var patientDtos = patients.Select(p => new PatientDto
            {
                PatientId = p.Id,
                Username = p.Username,
                Email = p.Email,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PhoneNumber = p.PhoneNumber,
                Gender = p.Gender,
                Height = p.Height,
                Weight = p.Weight,
                DateOfBirth = p.DateOfBirth,
                DateOfRegistration = p.DateOfRegistration,
                MedicalHistories = p.MedicalHistories.Select(mh => new MedicalHistoryDto
                {
                    HistoryId = mh.HistoryId,
                    DateOfDiagnosis = mh.DateOfDiagnosis,
                    Condition = mh.Condition,
                    PatientId = mh.PatientId
                }).ToList(),
                MedicalRecords = p.MedicalRecords.Select(mr => new MedicalRecordDto
                {
                    RecordId = mr.RecordId,
                    VisitReason = mr.VisitReason,
                    Symptoms = mr.Symptoms,
                    Diagnosis = mr.Diagnosis,
                    DoctorNotes = mr.DoctorNotes,
                    DoctorId = mr.DoctorId,
                    PatientId = mr.PatientId,
                    DateOfVisit = mr.DateOfVisit
                }).ToList()
            }).ToList();

            // Mock the mapper to handle any list of patients
            _mapperMock.Map<List<PatientDto>>(Arg.Any<IEnumerable<Domain.Entities.Patient>>())
                .Returns(patientDtos);

            var query = new GetPaginatedPatientsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Data);
            Assert.Equal(patientDtos.Count, result.Data.Data.Count);
            Assert.Equal(patients.Count, result.Data.TotalCount);
        }
    }
}
