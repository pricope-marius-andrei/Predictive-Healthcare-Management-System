using Application.UseCases.Commands.MedicalHistory;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalHistory;

public class DeleteMedicalHistoryCommandHandler : IRequestHandler<DeleteMedicalHistoryCommand>
{
    private readonly IMedicalHistoryRepository _repository;

    public DeleteMedicalHistoryCommandHandler(IMedicalHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = await _repository.GetByIdAsync(request.HistoryId);
        if (medicalHistory == null)
        {
            throw new InvalidOperationException("Medical history not found.");
        }

        await _repository.DeleteAsync(request.HistoryId);
    }
}
