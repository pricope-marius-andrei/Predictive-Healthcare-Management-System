using Domain.Entities;
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Patient patient);
        Task<Result<Patient>> UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);
    }
}
