using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(Guid id);
        Task<Result<Doctor>> UpdateAsync(Doctor doctor);
        Task DeleteAsync(Guid id);
        Task<int> CountAsync(IEnumerable<Doctor> query);
        Task<List<Doctor>> GetPaginatedAsync(IEnumerable<Doctor> query, int page, int pageSize);
    }
}
