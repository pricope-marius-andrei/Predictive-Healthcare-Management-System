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
            var patientsResult = await _repository.GetAllAsync();

            if (!patientsResult.IsSuccess || patientsResult.Data == null)
            {
                return Result<PagedResult<PatientDto>>.Failure("Failed to retrieve patients.");
            }

            var patientsList = patientsResult.Data.ToList(); // Copy patients.Data to patientsList

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                patientsList = patientsList
                    .Where(p => p.Username != null && p.Username.Contains(request.Username, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var totalCountResult = await _repository.CountAsync(patientsList);

            if (!totalCountResult.IsSuccess)
            {
                return Result<PagedResult<PatientDto>>.Failure("Failed to count patients.");
            }

            var pagedPatientsResult = await _repository.GetPaginatedAsync(patientsList, request.Page, request.PageSize);

            if (!pagedPatientsResult.IsSuccess || pagedPatientsResult.Data == null)
            {
                return Result<PagedResult<PatientDto>>.Failure("Failed to retrieve paginated patients.");
            }

            var patientDtos = _mapper.Map<List<PatientDto>>(pagedPatientsResult.Data);

            var pagedResult = new PagedResult<PatientDto>(patientDtos, totalCountResult.Data);

            return Result<PagedResult<PatientDto>>.Success(pagedResult);
        }

    }
}
