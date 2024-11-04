using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(Guid id);
    }
}
