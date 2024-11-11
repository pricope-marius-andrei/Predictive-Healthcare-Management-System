using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetMedicalHistoryByIdQueryHandler : IRequestHandler<GetMedicalHistoryByIdQuery, MedicalHistoryDto>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalHistoryByIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MedicalHistoryDto> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var medicalHistory = await _repository.GetByIdAsync(request.HistoryId);

            if (medicalHistory == null)
            {
                throw new KeyNotFoundException("Medical history not found.");
            }

            return _mapper.Map<MedicalHistoryDto>(medicalHistory);
        }
    }
}
