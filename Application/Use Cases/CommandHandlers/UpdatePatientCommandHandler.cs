using Application.Use_Cases.Commands;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
{
    private readonly IPatientRepository _repository;

    public UpdatePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repository.GetByIdAsync(request.PatientId);
        if (patient == null)
        {
            return false;
        }

        patient.Username = request.Username;
        patient.Email = request.Email;
        patient.Password = request.Password;
        patient.FirstName = request.FirstName;
        patient.LastName = request.LastName;
        patient.PhoneNumber = request.PhoneNumber;
        patient.Address = request.Address;
        patient.Gender = request.Gender;
        patient.Height = request.Height;
        patient.Weight = request.Weight;

        await _repository.UpdateAsync(patient);
        return true;
    }
}