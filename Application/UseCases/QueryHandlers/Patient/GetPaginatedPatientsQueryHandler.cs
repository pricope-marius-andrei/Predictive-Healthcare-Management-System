using Application.DTOs;
using Application.UseCases.Queries.Patient;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Patient
{
    public class GetPaginatedPatientsQueryHandler : IRequestHandler<GetPaginatedPatientsQuery, Result<PagedResult<PatientDto>>>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetPaginatedPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<PatientDto>>> Handle(GetPaginatedPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _repository.GetAllAsync();

            var patientsList = patients.Data;

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                patientsList = patientsList
                    .Where(p => p.Username.Contains(request.Username, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = await _repository.CountAsync(patientsList);

            var pagedPatients = await _repository.GetPaginatedAsync(patientsList, request.Page, request.PageSize);

            var patientDtos = _mapper.Map<List<PatientDto>>(pagedPatients);

            var pagedResult = new PagedResult<PatientDto>(patientDtos, totalCount.Data);

            return Result<PagedResult<PatientDto>>.Success(pagedResult);
        }
    }
}