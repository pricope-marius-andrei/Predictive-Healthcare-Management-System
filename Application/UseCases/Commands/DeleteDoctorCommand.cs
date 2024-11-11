using MediatR;

namespace Application.UseCases.Commands
{
    public class DeleteDoctorCommand : IRequest
    {
        public Guid DoctorId { get; set; }
    }
}
