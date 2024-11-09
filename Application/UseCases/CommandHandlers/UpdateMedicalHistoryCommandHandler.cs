using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class UpdateMedicalHistoryCommandHandler : IRequestHandler<UpdateMedicalHistoryCommand, Result<MedicalHistory>>
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IMapper _mapper;

    public UpdateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<MedicalHistory>> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = _mapper.Map<MedicalHistory>(request);

        var result = await _repository.UpdateAsync(medicalHistory);
        if (result.IsSuccess)
        {
            return Result<MedicalHistory>.Success(result.Data);
        }
        return Result<MedicalHistory>.Failure(result.ErrorMessage);
    }
}

