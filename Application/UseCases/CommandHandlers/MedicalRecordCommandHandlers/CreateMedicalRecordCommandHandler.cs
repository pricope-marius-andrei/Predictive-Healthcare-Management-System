using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class CreateMedicalRecordCommandHandler(
        IMedicalRecordRepository repository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper)
        : IRequestHandler<CreateMedicalRecordCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var patientExists = await patientRepository.GetByIdAsync(request.PatientId) != null;
			if (!patientExists)
			{
				return Result<Guid>.Failure("Patient not found.");
			}

			var doctorExists = await doctorRepository.GetByIdAsync(request.DoctorId) != null;
			if (!doctorExists)
			{
				return Result<Guid>.Failure("Doctor not found.");
			}

			var medicalRecord = mapper.Map<MedicalRecord>(request);

			var result = await repository.AddAsync(medicalRecord);
			if (result.IsSuccess)
			{
				return Result<Guid>.Success(result.Data);
			}

			return Result<Guid>.Failure(result.ErrorMessage);
		}
	}
}