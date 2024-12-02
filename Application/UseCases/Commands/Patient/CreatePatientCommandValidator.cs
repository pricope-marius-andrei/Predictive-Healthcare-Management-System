using Application.UseCases.Commands.Base;
using Domain.Common;

namespace Application.UseCases.Commands.Patient
{
    public class CreatePatientCommandValidator : BaseUserCommandValidator<CreatePatientCommand, Result<Guid>>
    {
        public CreatePatientCommandValidator() : base()
        {

        }
    }
}
