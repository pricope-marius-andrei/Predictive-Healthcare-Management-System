using Application.UseCases.Commands.Base;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.Patient
{
    public class CreatePatientCommand : BasePatientCommand<Result<Guid>>
    {

    }
}