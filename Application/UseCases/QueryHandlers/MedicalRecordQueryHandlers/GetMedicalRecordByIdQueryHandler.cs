using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers
{
	public class GetMedicalRecordByIdQueryHandler : IRequestHandler<GetMedicalRecordByIdQuery, Result<MedicalRecordDto>>
	{
		private readonly IMedicalRecordRepository _repository;
		private readonly IMapper _mapper;

		public GetMedicalRecordByIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<MedicalRecordDto>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
		{
			var medicalRecord = await _repository.GetByIdAsync(request.RecordId);

			if (medicalRecord == null)
			{
				return Result<MedicalRecordDto>.Failure("Medical record not found.");
			}

			var medicalRecordDto = _mapper.Map<MedicalRecordDto>(medicalRecord);
			return Result<MedicalRecordDto>.Success(medicalRecordDto);
		}
	}
}