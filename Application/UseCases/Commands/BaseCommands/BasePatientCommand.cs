namespace Application.UseCases.Commands.BaseCommands
{
    public class BasePatientCommand<T> : BaseCommand<T>
    {
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}