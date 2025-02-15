﻿using Application.UseCases.Commands.Doctor;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.Doctor
{
    public class RemovePatientFromDoctorCommandHandler : IRequestHandler<RemovePatientFromDoctorCommand, Result<Domain.Entities.Doctor>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public RemovePatientFromDoctorCommandHandler(IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Domain.Entities.Doctor>> Handle(RemovePatientFromDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (!doctor.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Doctor not found.");
            }

            var patient = await _patientRepository.GetByIdAsync(request.PatientId);
            if (!patient.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Patient not found.");
            }

            if (patient.Data.DoctorId != doctor.Data.Id)
            {
                return Result<Domain.Entities.Doctor>.Failure("Patient is not assigned to this doctor.");
            }

            //patient.Data.DoctorId = Guid.Empty; 

            var updateResult = await _patientRepository.UpdateAsync(patient.Data);
            if (!updateResult.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Failed to remove patient from doctor.");
            }

            if (doctor.Data.Patients != null)
            {
                doctor.Data.Patients.Remove(patient.Data);
            }

            var doctorUpdateResult = await _doctorRepository.UpdateAsync(doctor.Data);
            if (!doctorUpdateResult.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Failed to update doctor's patient list.");
            }

            return Result<Domain.Entities.Doctor>.Success(doctor.Data);
        }
    }
}
