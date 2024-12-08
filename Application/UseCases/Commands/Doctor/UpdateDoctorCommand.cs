using Application.UseCases.Commands.Base;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.Doctor
{
    public class UpdateDoctorCommand : BaseDoctorCommand<Result<Domain.Entities.Doctor>>, IRequest<Result<Domain.Entities.Doctor>>
    {
        public Guid Id { get; set; }
    }
}