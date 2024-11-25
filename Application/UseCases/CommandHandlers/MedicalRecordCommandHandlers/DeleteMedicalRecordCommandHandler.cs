using Application.UseCases.Commands.MedicalRecordCommands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class DeleteMedicalRecordCommandHandler(IMedicalRecordRepository repository)
        : IRequestHandler<DeleteMedicalRecordCommand>
    {
        public async Task Handle(DeleteMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var medicalRecord = await repository.GetByIdAsync(request.RecordId);
			if (medicalRecord == null)
			{
				throw new InvalidOperationException("Medical record not found.");
			}

			await repository.DeleteAsync(request.RecordId);
		}
	}
}