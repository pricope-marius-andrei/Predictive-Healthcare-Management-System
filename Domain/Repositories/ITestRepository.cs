using Domain.Entities;

namespace Domain.Repositories
{
    public interface ITestRepository
    {
        Task<Guid> AddAsync(Test test);
    }
}
