using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdatePatientCommand : CreatePatientCommand, IRequest
    {
        public Guid PatientId { get; set; }
    }
}