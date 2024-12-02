using Application.UseCases.Commands.Patient;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.Patient;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IPatientRepository _repository;

    public DeletePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.PatientId);
        if (patient == null)
        {
            throw new InvalidOperationException("Patient not found.");
        }

        await _repository.DeleteAsync(request.PatientId);
    }
}