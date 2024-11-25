using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class UpdateMedicalRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
        : IRequestHandler<UpdateMedicalRecordCommand, Result<MedicalRecord>>
    {
        public async Task<Result<MedicalRecord>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var existingMedicalRecord = await medicalRecordRepository.GetByIdAsync(request.RecordId);

			if (existingMedicalRecord == null)
			{
				return Result<MedicalRecord>.Failure("Medical record not found.");
			}

			mapper.Map(request, existingMedicalRecord);

			var updateResult = await medicalRecordRepository.UpdateAsync(existingMedicalRecord);

			if (!updateResult.IsSuccess)
			{
				return Result<MedicalRecord>.Failure(updateResult.ErrorMessage);
			}

			return Result<MedicalRecord>.Success(updateResult.Data);
		}
	}
}