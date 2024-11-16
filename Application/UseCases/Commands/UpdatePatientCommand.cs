using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdatePatientCommand : BasePatientCommand<Result<Patient>>
    {
        public Guid PatientId { get; set; }
    }
}