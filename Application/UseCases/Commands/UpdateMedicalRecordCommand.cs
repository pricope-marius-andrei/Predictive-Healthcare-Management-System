using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateMedicalRecordCommand : IRequest<Result<MedicalRecord>>
    {
        public Guid RecordId { get; set; }
        public string VisitReason { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorNotes { get; set; }
        public DateTime DateOfVisit { get; set; } = DateTime.Now;
    }
}

