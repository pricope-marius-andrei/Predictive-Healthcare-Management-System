using Application.Use_Cases.Commands;
using Domain.Entities;
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
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address
        };  
        return await repository.AddAsync(patient);  
    }
}

