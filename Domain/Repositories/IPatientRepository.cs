using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(Guid id);
        Task<Result<Patient>> UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(Guid doctorId);
        Task<int> CountAsync(IEnumerable<Patient> query);
        Task<List<Patient>> GetPaginatedAsync(IEnumerable<Patient> query, int page, int pageSize);
    }
}
