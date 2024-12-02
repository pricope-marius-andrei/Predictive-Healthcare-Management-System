using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.MedicalHistory
{
    public class UpdateMedicalHistoryCommand : IRequest<Result<Domain.Entities.MedicalHistory>>
    {
        public Guid HistoryId { get; set; }
        public string? Condition { get; set; }
        public DateTime DateOfDiagnosis { get; set; } = DateTime.Now;
    }
}