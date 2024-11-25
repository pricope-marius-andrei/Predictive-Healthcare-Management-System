using Application.UseCases.Commands.PatientCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.PatientCommandHandlers
{
	public class CreatePatientCommandHandler(
        IPatientRepository repository,
        IMapper mapper,
        IValidator<CreatePatientCommand> validator)
        : IRequestHandler<CreatePatientCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await validator.ValidateAsync(request, cancellationToken);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			var patient = mapper.Map<Patient>(request);
			var result = await repository.AddAsync(patient);

			if (result.IsSuccess)
			{
				return Result<Guid>.Success(result.Data);
			}

			return Result<Guid>.Failure(result.ErrorMessage);
		}
	}
}