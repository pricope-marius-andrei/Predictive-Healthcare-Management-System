using Application.DTOs;
using Application.UseCases.Queries.MedicalRecord;
using Application.UseCases.QueryHandlers.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic; // Add this using directive
using System.Threading;
using System.Threading.Tasks; // Add this using directive

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class GetPaginatedMedicalRecordsQueryHandlerTests
    {
        private readonly IMedicalRecordRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly GetPaginatedMedicalRecordsQueryHandler _handler;

        public GetPaginatedMedicalRecordsQueryHandlerTests()
        {
            _repositoryMock = Substitute.For<IMedicalRecordRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new GetPaginatedMedicalRecordsQueryHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WhenMedicalRecordsExist()
        {
            // Arrange
            var medicalRecord1Id = Guid.NewGuid();
            var medicalRecord2Id = Guid.NewGuid();
            var doctor1Id = Guid.NewGuid();
            var doctor2Id = Guid.NewGuid();
            var patient1Id = Guid.NewGuid();
            var patient2Id = Guid.NewGuid();

            var medicalRecords = new List<Domain.Entities.MedicalRecord>
            {
                new Domain.Entities.MedicalRecord
                {
                    RecordId = medicalRecord1Id,
                    VisitReason = "Annual Checkup",
                    Symptoms = "None",
                    Diagnosis = "Healthy",
                    DoctorNotes = "No issues",
                    DoctorId = doctor1Id,
                    PatientId = patient1Id,
                    DateOfVisit = DateTime.UtcNow.AddMonths(-2)
                },
                new Domain.Entities.MedicalRecord
                {
                    RecordId = medicalRecord2Id,
                    VisitReason = "Flu Symptoms",
                    Symptoms = "Fever, Cough",
                    Diagnosis = "Influenza",
                    DoctorNotes = "Prescribed medication",
                    DoctorId = doctor2Id,
                    PatientId = patient2Id,
                    DateOfVisit = DateTime.UtcNow.AddMonths(-1)
                }
            };

            // Mock repository to return the list of medical records
            _repositoryMock.GetAllAsync().Returns(Task.FromResult(Result<IEnumerable<Domain.Entities.MedicalRecord>>.Success(medicalRecords)));

            // Prepare the expected DTOs
            var medicalRecordDtos = medicalRecords.Select(m => new MedicalRecordDto
            {
                RecordId = m.RecordId,
                VisitReason = m.VisitReason,
                Symptoms = m.Symptoms,
                Diagnosis = m.Diagnosis,
                DoctorNotes = m.DoctorNotes,
                DoctorId = m.DoctorId,
                PatientId = m.PatientId,
                DateOfVisit = m.DateOfVisit
            }).ToList();

            // Mock the mapper to handle any list of medical records
            _mapperMock.Map<List<MedicalRecordDto>>(Arg.Any<IEnumerable<Domain.Entities.MedicalRecord>>())
                .Returns(callInfo =>
                {
                    var inputRecords = callInfo.Arg<IEnumerable<Domain.Entities.MedicalRecord>>();
                    return inputRecords.Select(m => new MedicalRecordDto
                    {
                        RecordId = m.RecordId,
                        VisitReason = m.VisitReason,
                        Symptoms = m.Symptoms,
                        Diagnosis = m.Diagnosis,
                        DoctorNotes = m.DoctorNotes,
                        DoctorId = m.DoctorId,
                        PatientId = m.PatientId,
                        DateOfVisit = m.DateOfVisit
                    }).ToList();
                });

            var query = new GetPaginatedMedicalRecordsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Data.Should().HaveCount(medicalRecordDtos.Count);
            result.Data.TotalCount.Should().Be(medicalRecords.Count);
        }
    }
}



