using Application.UseCases.Commands.PatientCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.PatientCommandHandlers
{
	public class UpdatePatientCommandHandler(
        IPatientRepository repository,
        IMapper mapper,
        IValidator<UpdatePatientCommand> validator)
        : IRequestHandler<UpdatePatientCommand, Result<Patient>>
    {
        public async Task<Result<Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await validator.ValidateAsync(request, cancellationToken);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			var patient = mapper.Map<Patient>(request);

			var result = await repository.UpdateAsync(patient);
			if (result.IsSuccess)
			{
				return Result<Patient>.Success(result.Data);
			}

			return Result<Patient>.Failure(result.ErrorMessage);
		}
	}
}