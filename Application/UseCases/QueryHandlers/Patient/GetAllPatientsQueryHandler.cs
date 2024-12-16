using Application.DTOs;
using Application.UseCases.Queries.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Patient
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, Result<IEnumerable<PatientDto>>>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patientsResult = await _repository.GetAllAsync();
            if (!patientsResult.IsSuccess)
            {
                return Result<IEnumerable<PatientDto>>.Failure(patientsResult.ErrorMessage);
            }

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patientsResult.Data);
            return Result<IEnumerable<PatientDto>>.Success(patientDtos);
        }
    }
}



















