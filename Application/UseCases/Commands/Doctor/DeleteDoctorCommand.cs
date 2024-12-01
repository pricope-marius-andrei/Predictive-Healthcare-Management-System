using MediatR;

namespace Application.UseCases.Commands.Doctor
{
    public class DeleteDoctorCommand : IRequest
    {
        public Guid DoctorId { get; set; }
    }
}
