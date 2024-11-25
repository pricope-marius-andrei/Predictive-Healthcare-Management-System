using Application.UseCases.Commands.MedicalHistoryCommands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalHistoryCommandHandlers
{
	public class DeleteMedicalHistoryCommandHandler(IMedicalHistoryRepository repository)
        : IRequestHandler<DeleteMedicalHistoryCommand>
    {
        public async Task Handle(DeleteMedicalHistoryCommand request, CancellationToken cancellationToken)
		{
			var medicalHistory = await repository.GetByIdAsync(request.HistoryId);
			if (medicalHistory == null)
			{
				throw new InvalidOperationException("Medical history not found.");
			}

			await repository.DeleteAsync(request.HistoryId);
		}
	}
}