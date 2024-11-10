using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Result<MedicalRecord>>
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<MedicalRecord>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var medicalRecord = _mapper.Map<MedicalRecord>(request);

        var result = await _repository.UpdateAsync(medicalRecord);
        if (result.IsSuccess)
        {
            return Result<MedicalRecord>.Success(result.Data);
        }
        return Result<MedicalRecord>.Failure(result.ErrorMessage);
    }
}


