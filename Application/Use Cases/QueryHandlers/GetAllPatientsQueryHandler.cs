using Application.DTOs;
using Application.Use_Cases.Queries;
using Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, IEnumerable<PatientsDto>>
    {
        private readonly IPatientRepository repository;

        public GetAllPatientsQueryHandler(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<PatientsDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await repository.GetAllAsync();

            return patients.Select(patient => new PatientsDto
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
                    Illness = mh.Illness,
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
            }).ToList();
        }
    }
}
