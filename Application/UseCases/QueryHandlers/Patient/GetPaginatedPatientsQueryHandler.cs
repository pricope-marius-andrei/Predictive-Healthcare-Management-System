using Application.DTOs;
using Application.UseCases.Queries.Patient;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
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
            var query = patients.AsQueryable();

            var pagedPatients = query.ApplyPaging(request.Page, request.PageSize);

            var patientDtos = _mapper.Map<List<PatientDto>>(pagedPatients);

            var pagedResult = new PagedResult<PatientDto>(patientDtos, query.Count());

            return Result<PagedResult<PatientDto>>.Success(pagedResult);
        }
    }
}