using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetPatientsByDoctorIdQueryHandler : IRequestHandler<GetPatientsByDoctorIdQuery, Result<PagedResult<PatientDto>>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public GetPatientsByDoctorIdQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<PatientDto>>> Handle(GetPatientsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetPatientsByDoctorIdAsync(request.DoctorId);

            var patientsList = patients.Data.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                patientsList = patientsList
                    .Where(p => p.Username.Contains(request.Username, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = patientsList.Count();

            var pagedPatients = patientsList
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var patientDtos = _mapper.Map<List<PatientDto>>(pagedPatients);

            var pagedResult = new PagedResult<PatientDto>(patientDtos, totalCount);

            return Result<PagedResult<PatientDto>>.Success(pagedResult);
        }
    }
}
