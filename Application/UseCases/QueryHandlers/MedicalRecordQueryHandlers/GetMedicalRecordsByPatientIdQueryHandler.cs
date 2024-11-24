using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers
{
	public class GetMedicalRecordsByPatientIdQueryHandler : IRequestHandler<GetMedicalRecordsByPatientIdQuery, Result<IEnumerable<MedicalRecordDto>>>
	{
		private readonly IMedicalRecordRepository _repository;
		private readonly IMapper _mapper;

		public GetMedicalRecordsByPatientIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Result<IEnumerable<MedicalRecordDto>>> Handle(GetMedicalRecordsByPatientIdQuery request, CancellationToken cancellationToken)
		{
			var medicalRecords = await _repository.GetByPatientIdAsync(request.PatientId);

			if (medicalRecords == null || !medicalRecords.Any())
			{
				return Result<IEnumerable<MedicalRecordDto>>.Failure("No medical records found for the specified patient.");
			}

			var medicalRecordDtos = _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
			return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
		}
	}
}