using Application.DTOs;
using Application.UseCases.Queries.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Patient
{
    public class GetPatientsSortedQueryHandler : IRequestHandler<GetPatientsSortedQuery, Result<List<PatientDto>>>
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public GetPatientsSortedQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<PatientDto>>> Handle(GetPatientsSortedQuery request, CancellationToken cancellationToken)
        {
            var patients = await _repository.GetAllAsync();

            List<Domain.Entities.Patient> sortedPatients;

            switch (request.SortBy)
            {
                case PatientSortBy.Username:
                    sortedPatients = patients.OrderBy(p => p.Username).ToList();
                    break;
                case PatientSortBy.Height:
                    sortedPatients = patients.OrderBy(p => p.Height).ToList();
                    break;
                case PatientSortBy.Weight:
                    sortedPatients = patients.OrderBy(p => p.Weight).ToList();
                    break;
                default:
                    return Result<List<PatientDto>>.Failure("Invalid sort attribute specified.");
            }

            var patientDtos = _mapper.Map<List<PatientDto>>(sortedPatients);

            return Result<List<PatientDto>>.Success(patientDtos);
        }
    }
}