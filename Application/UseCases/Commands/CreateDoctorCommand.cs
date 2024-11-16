using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommand : BaseDoctorCommand<Result<Guid>>, IRequest<Result<Guid>>
    {
        public CreateDoctorCommand() { }
    }
}