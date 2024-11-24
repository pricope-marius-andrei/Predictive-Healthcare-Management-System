using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.DoctorCommands
{
	public class CreateDoctorCommand : BaseDoctorCommand<Result<Guid>>, IRequest<Result<Guid>>
	{
		public CreateDoctorCommand()
		{

		}
	}
}