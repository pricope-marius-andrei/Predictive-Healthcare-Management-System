using MediatR;

namespace Application.UseCases.Commands.DoctorCommands
{
	public class DeleteDoctorCommand : IRequest
	{
		public Guid PersonId { get; set; }
	}
}