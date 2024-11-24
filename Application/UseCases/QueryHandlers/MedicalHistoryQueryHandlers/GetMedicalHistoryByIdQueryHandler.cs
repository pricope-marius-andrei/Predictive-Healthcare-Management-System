using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
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
			var medicalHistory = await _repository.GetByIdAsync(request.HistoryId);

			if (medicalHistory == null)
			{
				return Result<MedicalHistoryDto>.Failure("Medical history not found.");
			}

			var medicalHistoryDto = _mapper.Map<MedicalHistoryDto>(medicalHistory);
			return Result<MedicalHistoryDto>.Success(medicalHistoryDto);
		}
	}
}