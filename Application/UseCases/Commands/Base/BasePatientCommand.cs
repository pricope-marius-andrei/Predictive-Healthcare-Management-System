namespace Application.UseCases.Commands.Base
{
    public class BasePatientCommand<T> : BaseCommand<T>
    {
        public string? Gender { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
