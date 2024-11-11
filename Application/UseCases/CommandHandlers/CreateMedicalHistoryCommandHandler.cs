using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class CreateMedicalHistoryCommandHandler : IRequestHandler<CreateMedicalHistoryCommand, Result<Guid>>
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public CreateMedicalHistoryCommandHandler(
        IMedicalHistoryRepository repository,
        IPatientRepository patientRepository,
        IMapper mapper)
    {
        _repository = repository;
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var patientExists = await _patientRepository.GetByIdAsync(request.PatientId) != null;
        if (!patientExists)
        {
            return Result<Guid>.Failure("Patient not found.");
        }

        var medicalHistory = _mapper.Map<MedicalHistory>(request);

        var result = await _repository.AddAsync(medicalHistory);
        if (result.IsSuccess)
        {
            return Result<Guid>.Success(result.Data);
        }
        return Result<Guid>.Failure(result.ErrorMessage);
    }
}
