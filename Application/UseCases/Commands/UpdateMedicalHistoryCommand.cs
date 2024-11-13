using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateMedicalHistoryCommand : IRequest<Result<MedicalHistory>>
    {
        public Guid HistoryId { get; set; }
        public string? Condition { get; set; }
        public DateTime DateOfDiagnosis { get; set; } = DateTime.Now;
    }
}
