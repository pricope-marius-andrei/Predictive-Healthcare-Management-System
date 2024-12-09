/*using Application.UseCases.Commands.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.Patient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreatePatientCommand> _validator;

    public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper, IValidator<CreatePatientCommand> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var patient = _mapper.Map<Domain.Entities.Patient>(request);
        var result = await _repository.AddAsync(patient);

        if (result.IsSuccess)
        {
            return Result<Guid>.Success(result.Data);
        }
        return Result<Guid>.Failure(result.ErrorMessage);
    }
}*/