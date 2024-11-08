using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Patient>>
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public UpdatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<Patient>(request);

        var result = await _repository.UpdateAsync(patient);
        if (result.IsSuccess)
        {
            return Result<Patient>.Success(result.Data);
        }
        return Result<Patient>.Failure(result.ErrorMessage);
    }
}
