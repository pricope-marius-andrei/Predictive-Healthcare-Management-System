using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Domain.Common;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetDoctorsSortedQueryHandler : IRequestHandler<GetDoctorsSortedQuery, Result<List<DoctorDto>>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public GetDoctorsSortedQueryHandler(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<DoctorDto>>> Handle(GetDoctorsSortedQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _repository.GetAllAsync();

            List<Domain.Entities.Doctor> sortedDoctors;

            switch (request.SortBy)
            {
                case DoctorSortBy.Username:
                    sortedDoctors = doctors.Data.OrderBy(d => d.Username).ToList();
                    break;
                case DoctorSortBy.FirstName:
                    sortedDoctors = doctors.Data.OrderBy(d => d.FirstName).ToList();
                    break;
                case DoctorSortBy.LastName:
                    sortedDoctors = doctors.Data.OrderBy(d => d.LastName).ToList();
                    break;
                default:
                    return Result<List<DoctorDto>>.Failure("Invalid sort attribute specified.");
            }

            var doctorDtos = _mapper.Map<List<DoctorDto>>(sortedDoctors);

            return Result<List<DoctorDto>>.Success(doctorDtos);
        }
    }
}