using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<Result<IEnumerable<Doctor>>> GetAllAsync();
        Task<Result<Doctor>> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Doctor doctor);
        Task<Result<Doctor>> UpdateAsync(Doctor doctor);
        Task DeleteAsync(Guid id);
        Task<Result<IEnumerable<Doctor>>> GetDoctorsByUsernameFilterAsync(string username);
    }
}
