using MediatR;

public class RegisterUserCommand : IRequest<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
