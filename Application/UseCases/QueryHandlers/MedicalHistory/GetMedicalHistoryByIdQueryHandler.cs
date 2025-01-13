using Application.DTOs;
using Application.UseCases.Queries.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.QueryHandlers.MedicalHistory
{
    public class GetMedicalHistoryByIdQueryHandler : IRequestHandler<GetMedicalHistoryByIdQuery, Result<MedicalHistoryDto>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetMedicalHistoryByIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<MedicalHistoryDto>> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var medicalHistoryResult = await _repository.GetByIdAsync(request.HistoryId);

            if (!medicalHistoryResult.IsSuccess || medicalHistoryResult.Data == null)
            {
                throw new KeyNotFoundException("Medical history not found");
            }

            var medicalHistoryDto = _mapper.Map<MedicalHistoryDto>(medicalHistoryResult.Data);

            return Result<MedicalHistoryDto>.Success(medicalHistoryDto);
        }
    }
}



