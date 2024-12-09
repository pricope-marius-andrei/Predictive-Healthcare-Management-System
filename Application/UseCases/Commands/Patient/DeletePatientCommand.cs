using MediatR;

namespace Application.UseCases.Commands.Patient
{
    public class DeletePatientCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}