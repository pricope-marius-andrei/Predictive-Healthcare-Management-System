﻿using Application.DTOs;
using Application.UseCases.Queries.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Patient
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetPatientByIdQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patientResult = await _repository.GetByIdAsync(request.Id);
            if (!patientResult.IsSuccess)
            {
                return Result<PatientDto>.Failure(patientResult.ErrorMessage);
            }

            var patientDto = _mapper.Map<PatientDto>(patientResult.Data);
            return Result<PatientDto>.Success(patientDto);
        }
    }
}





















