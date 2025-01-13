using Application.DTOs;
using Application.UseCases.Queries.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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

            if (!patientResult.IsSuccess || patientResult.Data == null)
            {
                throw new KeyNotFoundException("Patient not found");
            }

            var patientDto = _mapper.Map<PatientDto>(patientResult.Data);

            return Result<PatientDto>.Success(patientDto);
        }
    }
}

