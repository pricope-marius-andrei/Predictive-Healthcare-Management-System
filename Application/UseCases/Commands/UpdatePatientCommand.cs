using Application.UseCases.Commands;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdatePatientCommand : CreatePatientCommand, IRequest
    {
        public Guid PatientId { get; set; }
    }
}