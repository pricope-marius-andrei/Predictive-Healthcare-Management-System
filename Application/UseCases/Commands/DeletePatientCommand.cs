using MediatR;

namespace Application.UseCases.Commands
{
    public class DeletePatientCommand : IRequest
    {
        public Guid PatientId { get; set; }
    }
}