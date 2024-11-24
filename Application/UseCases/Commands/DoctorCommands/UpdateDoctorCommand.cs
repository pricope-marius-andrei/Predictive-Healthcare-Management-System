using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.DoctorCommands
{
	public class UpdateDoctorCommand : BaseDoctorCommand<Result<Doctor>>, IRequest<Result<Doctor>>
	{
		public Guid DoctorId { get; set; }
	}
}