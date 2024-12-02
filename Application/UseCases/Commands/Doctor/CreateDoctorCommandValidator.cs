using Application.UseCases.Commands.Base;
using Domain.Common;

namespace Application.UseCases.Commands.Doctor
{
    public class CreateDoctorCommandValidator : BaseUserCommandValidator<CreateDoctorCommand, Result<Guid>>
    {
        public CreateDoctorCommandValidator() : base()
        {

        }
    }
}
