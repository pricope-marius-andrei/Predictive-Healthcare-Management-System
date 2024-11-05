using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IPatientRepository repository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CreatePatientCommandHandler(IPatientRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        this.repository = repository;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = Guid.Parse(userIdClaim.Value);

        var patient = new Patient
        {
            PatientId = userId,
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

        return await repository.AddAsync(patient);
    }
}