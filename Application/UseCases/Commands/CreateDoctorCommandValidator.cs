namespace Application.UseCases.Commands
{
	public class CreateDoctorCommandValidator : BaseDoctorCommandValidator<BaseDoctorCommand>
	{
		public CreateDoctorCommandValidator()
		{
			AddDoctorRules();
		}
	}
}