using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers
{
	public class GetAllMedicalRecordsQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        : IRequestHandler<GetAllMedicalRecordsQuery, Result<IEnumerable<MedicalRecordDto>>>
    {
        public async Task<Result<IEnumerable<MedicalRecordDto>>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
		{
			var medicalRecords = await repository.GetAllAsync();

			if (medicalRecords == null || !medicalRecords.Any())
			{
				return Result<IEnumerable<MedicalRecordDto>>.Failure("No medical records found.");
			}

			var medicalRecordDtos = mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
			return Result<IEnumerable<MedicalRecordDto>>.Success(medicalRecordDtos);
		}
	}
}