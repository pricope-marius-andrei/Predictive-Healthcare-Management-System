using Application.UseCases.Commands.DoctorCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.DoctorCommandHandlers
{
    public class UpdateDoctorCommandHandler(
        IDoctorRepository repository,
        IMapper mapper,
        IValidator<UpdateDoctorCommand> validator)
        : IRequestHandler<UpdateDoctorCommand, Result<Doctor>>
    {
        public async Task<Result<Doctor>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var doctor = mapper.Map<Doctor>(request);

            var result = await repository.UpdateAsync(doctor);
            if (result.IsSuccess)
            {
                return Result<Doctor>.Success(result.Data);
            }
            return Result<Doctor>.Failure(result.ErrorMessage);
        }
    }
}