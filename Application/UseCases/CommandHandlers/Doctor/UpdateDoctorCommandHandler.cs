using Application.UseCases.Commands.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers.Doctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result<Domain.Entities.Doctor>>
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

        public async Task<Result<Domain.Entities.Doctor>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingDoctor = await _repository.GetByIdAsync(request.Id);
            if (existingDoctor == null)
            {
                return Result<Domain.Entities.Doctor>.Failure("Doctor not found.");
            }

            _mapper.Map(request, existingDoctor);

            var updateResult = await _repository.UpdateAsync(existingDoctor);
            if (updateResult.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Success(updateResult.Data);
            }

            return Result<Domain.Entities.Doctor>.Failure(updateResult.ErrorMessage);

            /*
            var doctor = _mapper.Map<Domain.Entities.Doctor>(request);

            var result = await _repository.UpdateAsync(doctor);
            if (result.IsSuccess)
            {
                return Result<Domain.Entities.Doctor>.Success(result.Data);
            }
            return Result<Domain.Entities.Doctor>.Failure(result.ErrorMessage);*/
        }
    }
}