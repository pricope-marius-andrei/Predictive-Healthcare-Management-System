using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Doctor doctor);
        Task<Result<Doctor>> UpdateAsync(Doctor doctor);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Doctor>> GetDoctorsByUsernameFilterAsync(string username);
    }
}
