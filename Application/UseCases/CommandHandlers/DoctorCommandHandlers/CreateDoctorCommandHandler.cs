using Application.UseCases.Commands.DoctorCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.DoctorCommandHandlers
{
    public class CreateDoctorCommandHandler(
        IDoctorRepository repository,
        IMapper mapper,
        IValidator<CreateDoctorCommand> validator)
        : IRequestHandler<CreateDoctorCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var doctor = mapper.Map<Doctor>(request);
            var result = await repository.AddAsync(doctor);

            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}