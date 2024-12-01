using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistory
{
    public class GetAllMedicalHistoriesQueryHandler : IRequestHandler<GetAllMedicalHistoriesQuery, IEnumerable<MedicalHistoryDto>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMedicalHistoriesQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalHistoryDto>> Handle(GetAllMedicalHistoriesQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
        }
    }
}

