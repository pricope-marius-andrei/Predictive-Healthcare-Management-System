using Application.UseCases.Commands.MedicalRecordCommands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class DeleteMedicalRecordCommandHandler : IRequestHandler<DeleteMedicalRecordCommand>
	{
		private readonly IMedicalRecordRepository _repository;

		public DeleteMedicalRecordCommandHandler(IMedicalRecordRepository repository)
		{
			_repository = repository;
		}

		public async Task Handle(DeleteMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var medicalRecord = await _repository.GetByIdAsync(request.RecordId);
			if (medicalRecord == null)
			{
				throw new InvalidOperationException("Medical record not found.");
			}

			await _repository.DeleteAsync(request.RecordId);
		}
	}
}