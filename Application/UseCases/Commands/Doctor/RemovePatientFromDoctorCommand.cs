using MediatR;
using Domain.Common;

namespace Application.UseCases.Commands.Doctor
{
    public class RemovePatientFromDoctorCommand : IRequest<Result<Domain.Entities.Doctor>>
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
    }
}