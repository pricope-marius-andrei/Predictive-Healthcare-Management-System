using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateMedicalHistoryCommand : IRequest<Result<Guid>>
    {
        public Guid PatientId { get; set; }
        public string? Condition { get; set; }
        public DateTime DateOfDiagnosis { get; set; } = DateTime.Now;

    }
}