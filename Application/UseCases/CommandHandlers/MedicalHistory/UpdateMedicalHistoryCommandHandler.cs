using Application.UseCases.CommandHandlers.MedicalHistory;
using Application.UseCases.Commands.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CommandHandlers.MedicalHistory
{
    public class UpdateMedicalHistoryCommandHandler : IRequestHandler<UpdateMedicalHistoryCommand, Result<Domain.Entities.MedicalHistory>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public UpdateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<Domain.Entities.MedicalHistory>> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
        {
            var existingMedicalHistoryResult = await _repository.GetByIdAsync(request.HistoryId);

            if (!existingMedicalHistoryResult.IsSuccess || existingMedicalHistoryResult.Data == null)
            {
                return Result<Domain.Entities.MedicalHistory>.Failure("Medical history not found.");
            }

            var existingMedicalHistory = existingMedicalHistoryResult.Data;

            var updatedMedicalHistory = _mapper.Map(request, existingMedicalHistory);

            var updateResult = await _repository.UpdateAsync(updatedMedicalHistory);

            if (!updateResult.IsSuccess)
            {
                return Result<Domain.Entities.MedicalHistory>.Failure(updateResult.ErrorMessage);
            }

            return Result<Domain.Entities.MedicalHistory>.Success(updateResult.Data);
        }
    }
}



