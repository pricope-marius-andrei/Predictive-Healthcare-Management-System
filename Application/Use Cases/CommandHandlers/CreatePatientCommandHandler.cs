using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientRepository repository;

    public CreatePatientCommandHandler(IPatientRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patientId = Guid.NewGuid();

        var patient = new Patient
        {
            PatientId = patientId,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Gender = request.Gender,
            Height = request.Height,
            Weight = request.Weight
        };

        await repository.AddAsync(patient);
        return patientId;
    }
}