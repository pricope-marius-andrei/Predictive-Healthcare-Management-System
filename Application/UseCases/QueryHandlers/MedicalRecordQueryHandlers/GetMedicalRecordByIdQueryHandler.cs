using Application.DTOs;
using Application.UseCases.Queries.MedicalRecordsQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.MedicalRecordQueryHandlers
{
	public class GetMedicalRecordByIdQueryHandler(IMedicalRecordRepository repository, IMapper mapper)
        : IRequestHandler<GetMedicalRecordByIdQuery, Result<MedicalRecordDto>>
    {
        public async Task<Result<MedicalRecordDto>> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
		{
			var medicalRecord = await repository.GetByIdAsync(request.RecordId);

			if (medicalRecord == null)
			{
				return Result<MedicalRecordDto>.Failure("Medical record not found.");
			}

			var medicalRecordDto = mapper.Map<MedicalRecordDto>(medicalRecord);
			return Result<MedicalRecordDto>.Success(medicalRecordDto);
		}
	}
}