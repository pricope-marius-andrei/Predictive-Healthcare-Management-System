using MediatR;

namespace Application.UseCases.Commands.Base
{
    public class BaseCommand<T> : IRequest<T>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
