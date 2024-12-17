using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetPaginatedDoctorsQueryHandler : IRequestHandler<GetPaginatedDoctorsQuery, Result<PagedResult<DoctorDto>>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public GetPaginatedDoctorsQueryHandler(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<DoctorDto>>> Handle(GetPaginatedDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                doctors = doctors
                    .Where(p => p.Username.Contains(request.Username, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = await _repository.CountAsync(doctors);

            var pagedDoctors = await _repository.GetPaginatedAsync(doctors, request.Page, request.PageSize);

            var doctorDtos = _mapper.Map<List<DoctorDto>>(pagedDoctors);

            var pagedResult = new PagedResult<DoctorDto>(doctorDtos, totalCount);

            return Result<PagedResult<DoctorDto>>.Success(pagedResult);
        }
    }
}