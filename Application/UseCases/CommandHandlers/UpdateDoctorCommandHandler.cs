using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result<Doctor>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateDoctorCommand> _validator;

        public UpdateDoctorCommandHandler(IDoctorRepository repository, IMapper mapper, IValidator<UpdateDoctorCommand> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<Doctor>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var doctor = _mapper.Map<Doctor>(request);

            var result = await _repository.UpdateAsync(doctor);
            if (result.IsSuccess)
            {
                return Result<Doctor>.Success(result.Data);
            }
            return Result<Doctor>.Failure(result.ErrorMessage);
        }
    }
}
