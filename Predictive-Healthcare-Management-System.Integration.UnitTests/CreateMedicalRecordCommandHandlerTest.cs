using Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers;
using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Integration.UnitTests
{
	public class CreateMedicalRecordCommandHandlerTest
	{
		private readonly IMedicalRecordRepository _medicalRecordRepository;
		private readonly IPatientRepository _patientRepository;
		private readonly IDoctorRepository _doctorRepository;
		private readonly IMapper _mapper;

		public CreateMedicalRecordCommandHandlerTest()
		{
			_medicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
			_patientRepository = Substitute.For<IPatientRepository>();
			_doctorRepository = Substitute.For<IDoctorRepository>();
			_mapper = Substitute.For<IMapper>();
		}

		[Fact]
		public async Task Handle_ShouldCreateMedicalRecord_WhenCommandIsValid()
		{
			// Arrange
			var command = new CreateMedicalRecordCommand
			{
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Routine Checkup",
				Symptoms = "None",
				Diagnosis = "Healthy",
				DoctorNotes = "No issues found",
				DateOfVisit = DateTime.UtcNow
			};

			var medicalRecord = new MedicalRecord
			{
				RecordId = Guid.NewGuid(),
				PatientId = command.PatientId,
				DoctorId = command.DoctorId,
				VisitReason = command.VisitReason,
				Symptoms = command.Symptoms,
				Diagnosis = command.Diagnosis,
				DoctorNotes = command.DoctorNotes,
				DateOfVisit = command.DateOfVisit
			};

			var doctor = new Doctor
			{
				PersonId = command.DoctorId,
				Username = "doctorUsername",
				Email = "doctor@example.com",
				Password = "password123",
				FirstName = "John",
				LastName = "Doe",
				PhoneNumber = "1234567890",
				Specialization = "Cardiology",
				DateOfRegistration = DateTime.UtcNow
			};

			_patientRepository.GetByIdAsync(command.PatientId).Returns(new Patient());
			_doctorRepository.GetByIdAsync(command.DoctorId).Returns(doctor);
			_mapper.Map<MedicalRecord>(command).Returns(medicalRecord);
			_medicalRecordRepository.AddAsync(medicalRecord).Returns(Result<Guid>.Success(medicalRecord.RecordId));

			// Act
			var handler = new CreateMedicalRecordCommandHandler(_medicalRecordRepository, _patientRepository, _doctorRepository, _mapper);
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(medicalRecord.RecordId, result.Data);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenPatientNotFound()
		{
			// Arrange
			var command = new CreateMedicalRecordCommand
			{
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Routine Checkup",
				Symptoms = "None",
				Diagnosis = "Healthy",
				DoctorNotes = "No issues found",
				DateOfVisit = DateTime.UtcNow
			};

			_patientRepository.GetByIdAsync(command.PatientId).Returns((Patient?)null);

			// Act
			var handler = new CreateMedicalRecordCommandHandler(_medicalRecordRepository, _patientRepository, _doctorRepository, _mapper);
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Patient not found.", result.ErrorMessage);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenDoctorNotFound()
		{
			// Arrange
			var command = new CreateMedicalRecordCommand
			{
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Routine Checkup",
				Symptoms = "None",
				Diagnosis = "Healthy",
				DoctorNotes = "No issues found",
				DateOfVisit = DateTime.UtcNow
			};

			_patientRepository.GetByIdAsync(command.PatientId).Returns(new Patient());
			_doctorRepository.GetByIdAsync(command.DoctorId).Returns((Doctor?)null);

			// Act
			var handler = new CreateMedicalRecordCommandHandler(_medicalRecordRepository, _patientRepository, _doctorRepository, _mapper);
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Doctor not found.", result.ErrorMessage);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
		{
			// Arrange
			var command = new CreateMedicalRecordCommand
			{
				PatientId = Guid.NewGuid(),
				DoctorId = Guid.NewGuid(),
				VisitReason = "Routine Checkup",
				Symptoms = "None",
				Diagnosis = "Healthy",
				DoctorNotes = "No issues found",
				DateOfVisit = DateTime.UtcNow
			};

			var medicalRecord = new MedicalRecord
			{
				RecordId = Guid.NewGuid(),
				PatientId = command.PatientId,
				DoctorId = command.DoctorId,
				VisitReason = command.VisitReason,
				Symptoms = command.Symptoms,
				Diagnosis = command.Diagnosis,
				DoctorNotes = command.DoctorNotes,
				DateOfVisit = command.DateOfVisit
			};

			var doctor = new Doctor
			{
				PersonId = command.DoctorId,
				Username = "doctorUsername",
				Email = "doctor@example.com",
				Password = "password123",
				FirstName = "John",
				LastName = "Doe",
				PhoneNumber = "1234567890",
				Specialization = "Cardiology",
				DateOfRegistration = DateTime.UtcNow
			};

			_patientRepository.GetByIdAsync(command.PatientId).Returns(new Patient());
			_doctorRepository.GetByIdAsync(command.DoctorId).Returns(doctor);
			_mapper.Map<MedicalRecord>(command).Returns(medicalRecord);
			_medicalRecordRepository.AddAsync(medicalRecord).Returns(Result<Guid>.Failure("Error adding medical record"));

			// Act
			var handler = new CreateMedicalRecordCommandHandler(_medicalRecordRepository, _patientRepository, _doctorRepository, _mapper);
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Error adding medical record", result.ErrorMessage);
		}
	}
}