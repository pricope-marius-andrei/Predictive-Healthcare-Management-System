using Application.UseCases.Commands.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.Doctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<Guid>>
    {
        readonly IDoctorRepository _repository;
        readonly IMapper _mapper;
        readonly IValidator<CreateDoctorCommand> _validator;

        public CreateDoctorCommandHandler(IDoctorRepository repository, IMapper mapper, IValidator<CreateDoctorCommand> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var doctor = _mapper.Map<Domain.Entities.Doctor>(request);
            var result = await _repository.AddAsync(doctor);

            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
