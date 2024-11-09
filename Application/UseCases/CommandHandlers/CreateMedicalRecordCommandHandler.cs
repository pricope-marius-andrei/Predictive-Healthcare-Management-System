using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class CreateMedicalHistoryCommandHandler : IRequestHandler<CreateMedicalHistoryCommand, Result<Guid>>
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IMapper _mapper;

    public CreateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
    {
        var medicalHistory = _mapper.Map<MedicalHistory>(request);
     
        var result = await _repository.AddAsync(medicalHistory);
        if (result.IsSuccess)
        {
            return Result<Guid>.Success(result.Data);
        }
        return Result<Guid>.Failure(result.ErrorMessage);
    }
}
