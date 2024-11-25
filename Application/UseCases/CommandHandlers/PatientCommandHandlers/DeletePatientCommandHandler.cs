using Application.UseCases.Commands.PatientCommands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.PatientCommandHandlers
{
	public class DeletePatientCommandHandler(IPatientRepository repository) : IRequestHandler<DeletePatientCommand>
    {
        public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
		{
			var patient = await repository.GetByIdAsync(request.PersonId);
			if (patient == null)
			{
				throw new InvalidOperationException("Patient not found.");
			}

			await repository.DeleteAsync(request.PersonId);
		}
	}
}