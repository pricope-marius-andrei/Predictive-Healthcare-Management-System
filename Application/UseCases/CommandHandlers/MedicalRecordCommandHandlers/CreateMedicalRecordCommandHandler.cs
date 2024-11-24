using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, Result<Guid>>
	{
		private readonly IMedicalRecordRepository _repository;
		private readonly IPatientRepository _patientRepository;
		private readonly IDoctorRepository _doctorRepository;
		private readonly IMapper _mapper;

		public CreateMedicalRecordCommandHandler(
			IMedicalRecordRepository repository,
			IPatientRepository patientRepository,
			IDoctorRepository doctorRepository,
			IMapper mapper)
		{
			_repository = repository;
			_patientRepository = patientRepository;
			_doctorRepository = doctorRepository;
			_mapper = mapper;
		}

		public async Task<Result<Guid>> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var patientExists = await _patientRepository.GetByIdAsync(request.PatientId) != null;
			if (!patientExists)
			{
				return Result<Guid>.Failure("Patient not found.");
			}

			var doctorExists = await _doctorRepository.GetByIdAsync(request.DoctorId) != null;
			if (!doctorExists)
			{
				return Result<Guid>.Failure("Doctor not found.");
			}

			var medicalRecord = _mapper.Map<MedicalRecord>(request);

			var result = await _repository.AddAsync(medicalRecord);
			if (result.IsSuccess)
			{
				return Result<Guid>.Success(result.Data);
			}

			return Result<Guid>.Failure(result.ErrorMessage);
		}
	}
}