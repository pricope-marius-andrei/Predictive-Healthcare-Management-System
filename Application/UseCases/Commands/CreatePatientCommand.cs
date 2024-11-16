using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreatePatientCommand : BasePatientCommand<Result<Guid>>
    {
        
    }
}