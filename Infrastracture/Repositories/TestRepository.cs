using Domain.Entities;
using Domain.Repositories;
using Infrastracture.Persistence;

namespace Infrastracture.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly ApplicationDbContext context;

        public TestRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Guid> AddAsync(Test test)
        {
            await context.Tests.AddAsync(test);
            await context.SaveChangesAsync();
            return test.Id;

        }
    }
}
