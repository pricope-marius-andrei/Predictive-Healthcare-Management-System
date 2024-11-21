using Domain.Common;

namespace Application.UseCases.Commands
{
    public class CreatePatientCommandValidator : BaseUserCommandValidator<CreatePatientCommand, Result<Guid>>
    {
        public CreatePatientCommandValidator() : base()
        { 
       
        }
    }
}
