using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommand : BaseDoctorCommand<Result<Doctor>>, IRequest<Result<Doctor>>
    {
        public Guid DoctorId { get; set; }
    }
}
