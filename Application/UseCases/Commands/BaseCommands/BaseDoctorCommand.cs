namespace Application.UseCases.Commands.BaseCommands
{
    public class BaseDoctorCommand<T> : BaseCommand<T>
    {
        public string? Specialization { get; set; }
    }
}
