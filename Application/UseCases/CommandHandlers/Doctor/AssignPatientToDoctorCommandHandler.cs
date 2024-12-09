using Application.UseCases.Commands.Doctor;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.Doctor
{
    public class AssignPatientToDoctorCommandHandler : IRequestHandler<AssignPatientToDoctorCommand, Result<Domain.Entities.Doctor>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AssignPatientToDoctorCommandHandler(IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Result<Domain.Entities.Doctor>> Handle(AssignPatientToDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

            var patient = await _patientRepository.GetByIdAsync(request.PatientId);

            patient.DoctorId = doctor.Id;

            var updateResult = await _patientRepository.UpdateAsync(patient);
            if (!updateResult.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Failed to assign patient to doctor.");
            }

            if (doctor.Patients == null)
            {
                doctor.Patients = new List<Domain.Entities.Patient>();
            }

            if (!doctor.Patients.Contains(patient))
            {
                doctor.Patients.Add(patient);
            }

            var doctorUpdateResult = await _doctorRepository.UpdateAsync(doctor);
            if (!doctorUpdateResult.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Failure("Failed to update doctor's patient list.");
            }

            return Result<Domain.Entities.Doctor>.Success(doctor);
        }
    }
}
