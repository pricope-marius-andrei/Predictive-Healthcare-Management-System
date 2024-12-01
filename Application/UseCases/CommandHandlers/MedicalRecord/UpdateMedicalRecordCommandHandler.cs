using Application.UseCases.Commands.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecord;

public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Result<Domain.Entities.MedicalRecord>>
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Domain.Entities.MedicalRecord>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var existingMedicalRecord = await _repository.GetByIdAsync(request.RecordId);
        if (existingMedicalRecord == null)
        {
            return Result<Domain.Entities.MedicalRecord>.Failure("Medical record not found.");
        }

        var medicalRecord = _mapper.Map<Domain.Entities.MedicalRecord>(request);
        medicalRecord.PatientId = existingMedicalRecord.PatientId;
        medicalRecord.DoctorId = existingMedicalRecord.DoctorId;

        var result = await _repository.UpdateAsync(medicalRecord);
        if (result.IsSuccess)
        {
            return Result<Domain.Entities.MedicalRecord>.Success(result.Data);
        }
        return Result<Domain.Entities.MedicalRecord>.Failure(result.ErrorMessage);
    }
}