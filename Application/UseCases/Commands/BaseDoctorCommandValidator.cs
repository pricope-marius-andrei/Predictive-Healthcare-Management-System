using FluentValidation;

namespace Application.UseCases.Commands
{
    public class BaseDoctorCommandValidator<T> : AbstractValidator<T> where T : CreateDoctorCommand
    {
        public BaseDoctorCommandValidator()
        {
            
        }
    }
}
