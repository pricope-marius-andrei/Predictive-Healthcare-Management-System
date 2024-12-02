using Application.UseCases.Commands.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalHistory;

public class UpdateMedicalHistoryCommandHandler : IRequestHandler<UpdateMedicalHistoryCommand, Result<Domain.Entities.MedicalHistory>>
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Domain.Entities.MedicalHistory>> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var existingMedicalHistory = await _repository.GetByIdAsync(request.HistoryId);
        if (existingMedicalHistory == null)
        {
            return Result<Domain.Entities.MedicalHistory>.Failure("Medical history not found.");
        }

        var medicalHistory = _mapper.Map<Domain.Entities.MedicalHistory>(request);
        medicalHistory.PatientId = existingMedicalHistory.PatientId;

        var result = await _repository.UpdateAsync(medicalHistory);
        if (result.IsSuccess)
        {
            return Result<Domain.Entities.MedicalHistory>.Success(result.Data);
        }
        return Result<Domain.Entities.MedicalHistory>.Failure(result.ErrorMessage);
    }
}