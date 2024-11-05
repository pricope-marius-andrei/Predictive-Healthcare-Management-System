using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
{
    private readonly IPatientRepository repository;

    public CreatePatientCommandHandler(IPatientRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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

        var result = await repository.AddAsync(patient);
        if (result.IsSuccess)
        {
            return Result<Guid>.Success(result.Data);
        }
        return Result<Guid>.Failure(result.ErrorMessage);
    }
}