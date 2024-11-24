using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
{
	public class GetAllMedicalHistoriesQueryHandler : IRequestHandler<GetAllMedicalHistoriesQuery, Result<IEnumerable<MedicalHistoryDto>>>
	{
		private readonly IMedicalHistoryRepository _medicalHistoryRepository;
		private readonly IMapper _mapper;

		public GetAllMedicalHistoriesQueryHandler(IMedicalHistoryRepository medicalHistoryRepository, IMapper mapper)
		{
			_medicalHistoryRepository = medicalHistoryRepository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<MedicalHistoryDto>>> Handle(GetAllMedicalHistoriesQuery request, CancellationToken cancellationToken)
		{
			var medicalHistories = await _medicalHistoryRepository.GetAllAsync();

			var medicalHistoryDtos = _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);

			return Result<IEnumerable<MedicalHistoryDto>>.Success(medicalHistoryDtos);
		}
	}
}