using Application.UseCases.Commands.MedicalRecordCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalRecordCommandHandlers
{
	public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Result<MedicalRecord>>
	{
		private readonly IMedicalRecordRepository _medicalRecordRepository;
		private readonly IMapper _mapper;

		public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
		{
			_medicalRecordRepository = medicalRecordRepository;
			_mapper = mapper;
		}

		public async Task<Result<MedicalRecord>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
		{
			var existingMedicalRecord = await _medicalRecordRepository.GetByIdAsync(request.RecordId);

			if (existingMedicalRecord == null)
			{
				return Result<MedicalRecord>.Failure("Medical record not found.");
			}

			_mapper.Map(request, existingMedicalRecord);

			var updateResult = await _medicalRecordRepository.UpdateAsync(existingMedicalRecord);

			if (!updateResult.IsSuccess)
			{
				return Result<MedicalRecord>.Failure(updateResult.ErrorMessage);
			}

			return Result<MedicalRecord>.Success(updateResult.Data);
		}
	}
}