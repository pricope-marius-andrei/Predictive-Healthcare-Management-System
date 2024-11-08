using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<Patient>(request);
     
        var result = await _repository.AddAsync(patient);
        if (result.IsSuccess)
        {
            return Result<Guid>.Success(result.Data);
        }
        return Result<Guid>.Failure(result.ErrorMessage);
    }
}
