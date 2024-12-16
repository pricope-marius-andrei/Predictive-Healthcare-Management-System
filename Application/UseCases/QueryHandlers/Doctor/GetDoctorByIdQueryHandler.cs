using Application.DTOs;
using Application.UseCases.Queries.Doctor;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.Doctor
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public GetDoctorByIdQueryHandler(IDoctorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctorResult = await _repository.GetByIdAsync(request.Id);
            if (!doctorResult.IsSuccess)
            {
                return Result<DoctorDto>.Failure(doctorResult.ErrorMessage);
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctorResult.Data);
            return Result<DoctorDto>.Success(doctorDto);
        }
    }
}











