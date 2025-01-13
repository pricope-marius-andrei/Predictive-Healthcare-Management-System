using Application.UseCases.CommandHandlers.MedicalRecord;
using Application.UseCases.Commands.MedicalRecord;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CommandHandlers.MedicalRecord
{
    public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Result<Domain.Entities.MedicalRecord>>
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly IMapper _mapper;

        public UpdateMedicalRecordCommandHandler(IMedicalRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<Domain.Entities.MedicalRecord>> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
        {
            var existingMedicalRecordResult = await _repository.GetByIdAsync(request.RecordId);

            if (!existingMedicalRecordResult.IsSuccess || existingMedicalRecordResult.Data == null)
            {
                return Result<Domain.Entities.MedicalRecord>.Failure("Medical record not found.");
            }

            var existingMedicalRecord = existingMedicalRecordResult.Data;

            var updatedMedicalRecord = _mapper.Map(request, existingMedicalRecord);

            var updateResult = await _repository.UpdateAsync(updatedMedicalRecord);

            if (!updateResult.IsSuccess)
            {
                return Result<Domain.Entities.MedicalRecord>.Failure(updateResult.ErrorMessage);
            }

            return Result<Domain.Entities.MedicalRecord>.Success(updateResult.Data);
        }
    }
}




