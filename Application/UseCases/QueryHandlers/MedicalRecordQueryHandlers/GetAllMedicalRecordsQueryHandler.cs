using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers
{
	public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, Result<IEnumerable<MedicalRecordDto>>>
	{
		private readonly IMedicalRecordRepository _repository;
		private readonly IMapper _mapper;

		public GetAllMedicalRecordsQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<MedicalRecordDto>>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
		{
			var medicalRecords = await _repository.GetAllAsync();

			if (medicalRecords == null || !medicalRecords.Any())
			{
				return Result<IEnumerable<MedicalRecordDto>>.Failure("No medical records found.");
			}

			var medicalRecordDtos = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
			return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
		}
	}
}