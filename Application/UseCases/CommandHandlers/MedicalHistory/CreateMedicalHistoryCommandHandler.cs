using Application.UseCases.Commands.MedicalHistory;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedicalHistoryEntity = Domain.Entities.MedicalHistory; // Add this alias

namespace Application.UseCases.CommandHandlers.MedicalHistory
{
    public class CreateMedicalHistoryCommandHandler : IRequestHandler<CreateMedicalHistoryCommand, Result<Guid>>
    {
        private readonly IMedicalHistoryRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public CreateMedicalHistoryCommandHandler(
            IMedicalHistoryRepository repository,
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
        {
            var patientResult = await _patientRepository.GetByIdAsync(request.PatientId);

            if (!patientResult.IsSuccess || patientResult.Data == null)
            {
                return Result<Guid>.Failure("Patient not found.");
            }

            var medicalHistory = _mapper.Map<MedicalHistoryEntity>(request); // Use the alias

            var addResult = await _repository.AddAsync(medicalHistory);

            if (!addResult.IsSuccess)
            {
                return Result<Guid>.Failure(addResult.ErrorMessage);
            }

            return Result<Guid>.Success(addResult.Data);
        }
    }
}




