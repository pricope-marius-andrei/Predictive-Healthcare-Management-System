using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.MedicalRecord
{
    public class UpdateMedicalRecordCommand : IRequest<Result<Domain.Entities.MedicalRecord>>
    {
        public Guid RecordId { get; set; }
        public string? VisitReason { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnosis { get; set; }
        public string? DoctorNotes { get; set; }
        public DateTime DateOfVisit { get; set; } = DateTime.Now;
    }
}