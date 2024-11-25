﻿using Application.UseCases.Commands.MedicalHistoryCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.MedicalHistoryCommandHandlers
{
	public class CreateMedicalHistoryCommandHandler(
        IMedicalHistoryRepository repository,
        IPatientRepository patientRepository,
        IMapper mapper)
        : IRequestHandler<CreateMedicalHistoryCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
	    {
	        var patientExists = await patientRepository.GetByIdAsync(request.PatientId) != null;
	        if (!patientExists)
	        {
	            return Result<Guid>.Failure("Patient not found.");
	        }

	        var medicalHistory = mapper.Map<MedicalHistory>(request);

	        var result = await repository.AddAsync(medicalHistory);
	        if (result.IsSuccess)
	        {
	            return Result<Guid>.Success(result.Data);
	        }
	        return Result<Guid>.Failure(result.ErrorMessage);
	    }
	}
}