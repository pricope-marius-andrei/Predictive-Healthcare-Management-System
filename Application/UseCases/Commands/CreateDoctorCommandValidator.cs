using Domain.Common;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommandValidator : BaseUserCommandValidator<CreateDoctorCommand, Result<Guid>>
    {
        public CreateDoctorCommandValidator() : base()
        {
            
        }
    }
}
