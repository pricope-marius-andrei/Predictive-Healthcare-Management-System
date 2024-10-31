using MediatR;

namespace Application.Use_Cases.Commands
{
    public class CreateTestCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}
