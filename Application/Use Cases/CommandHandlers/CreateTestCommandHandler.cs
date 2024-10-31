using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, Guid>
    {
        private readonly ITestRepository repository;

        public CreateTestCommandHandler(ITestRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Guid> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var test = new Test
            {
                Name = request.Name
            };

            return await repository.AddAsync(test);
        }
    }
}
