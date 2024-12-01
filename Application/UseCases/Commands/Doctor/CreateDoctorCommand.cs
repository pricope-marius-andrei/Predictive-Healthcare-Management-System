using Application.UseCases.Commands.Base;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.Doctor
{
    public class CreateDoctorCommand : BaseDoctorCommand<Result<Guid>>, IRequest<Result<Guid>>
    {
        public CreateDoctorCommand() { }
    }
}