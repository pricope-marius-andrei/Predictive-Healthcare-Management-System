using Application.DTOs;
using Application.UseCases.Queries.MedicalHistoryQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalHistoryQueryHandlers
{
	public class GetMedicalHistoriesByPatientIdQueryHandler : IRequestHandler<GetMedicalHistoriesByPatientIdQuery, Result<IEnumerable<MedicalHistoryDto>>>
	{
		private readonly IMedicalHistoryRepository _repository;
		private readonly IMapper _mapper;

		public GetMedicalHistoriesByPatientIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<MedicalHistoryDto>>> Handle(GetMedicalHistoriesByPatientIdQuery request, CancellationToken cancellationToken)
		{
			var medicalHistories = await _repository.GetByPatientIdAsync(request.PatientId);

			if (medicalHistories == null || !medicalHistories.Any())
			{
				return Result<IEnumerable<MedicalHistoryDto>>.Failure("No medical histories found for the specified patient.");
			}

			var medicalHistoryDtos = _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
			return Result<IEnumerable<MedicalHistoryDto>>.Success(medicalHistoryDtos);
		}
	}
}