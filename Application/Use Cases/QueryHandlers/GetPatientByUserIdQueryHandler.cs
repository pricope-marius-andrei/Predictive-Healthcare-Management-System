using Application.DTOs;
using Application.Use_Cases.Queries;
using Domain.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetPatientByUserIdQueryHandler : IRequestHandler<GetPatientByUserIdQuery, PatientsDto>
    {
        private readonly IPatientRepository repository;

        public GetPatientByUserIdQueryHandler(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PatientsDto> Handle(GetPatientByUserIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await repository.GetByIdAsync(request.Id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            return new PatientsDto
            {
                UserId = patient.PatientId,
                Username = patient.Username,
                Email = patient.Email,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                MedicalHistories = patient.MedicalHistories.Select(mh => new MedicalHistoryDto
                {
                    HistoryId = mh.HistoryId,
                    Illness = mh.Condition,
                    DateOfDiagnosis = mh.DateOfDiagnosis
                }).ToList(),
                MedicalRecords = patient.MedicalRecords.Select(mr => new MedicalRecordDto
                {
                    RecordId = mr.RecordId,
                    VisitReason = mr.VisitReason,
                    Symptoms = mr.Symptoms,
                    Diagnosis = mr.Diagnosis,
                    DoctorNotes = mr.DoctorNotes,
                    DoctorId = mr.DoctorId
                }).ToList()
            };
        }
    }
}