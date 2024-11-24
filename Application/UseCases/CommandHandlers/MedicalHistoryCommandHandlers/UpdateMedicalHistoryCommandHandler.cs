using Application.UseCases.Commands.MedicalHistoryCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalHistoryCommandHandlers;

public class UpdateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
	: IRequestHandler<UpdateMedicalHistoryCommand, Result<MedicalHistory>>
{
	public async Task<Result<MedicalHistory>> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var existingMedicalHistory = await repository.GetByIdAsync(request.HistoryId);
        if (existingMedicalHistory == null)
        {
            return Result<MedicalHistory>.Failure("Medical history not found.");
        }

        var medicalHistory = mapper.Map<MedicalHistory>(request);
        medicalHistory.PatientId = existingMedicalHistory.PatientId;

        var result = await repository.UpdateAsync(medicalHistory);
        if (result.IsSuccess)
        {
            return Result<MedicalHistory>.Success(result.Data);
        }
        return Result<MedicalHistory>.Failure(result.ErrorMessage);
    }
}