﻿using Application.UseCases.Commands.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.Patient;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Domain.Entities.Patient>>
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdatePatientCommand> _validator;

    public UpdatePatientCommandHandler(IPatientRepository repository, IMapper mapper, IValidator<UpdatePatientCommand> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<Domain.Entities.Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingPatient = await _repository.GetByIdAsync(request.Id);
        if (existingPatient == null)
        {
            return Result<Domain.Entities.Patient>.Failure("Patient not found.");
        }

        _mapper.Map(request, existingPatient);

        var updateResult = await _repository.UpdateAsync(existingPatient);
        if (updateResult.IsSuccess)
        {
            return Result<Domain.Entities.Patient>.Success(updateResult.Data);
        }

        return Result<Domain.Entities.Patient>.Failure(updateResult.ErrorMessage);

        /*var patient = _mapper.Map<Domain.Entities.Patient>(request);

        var result = await _repository.UpdateAsync(patient);
        if (result.IsSuccess)
        {
            return Result<Domain.Entities.Patient>.Success(result.Data);
        }
        return Result<Domain.Entities.Patient>.Failure(result.ErrorMessage);*/
    }
}