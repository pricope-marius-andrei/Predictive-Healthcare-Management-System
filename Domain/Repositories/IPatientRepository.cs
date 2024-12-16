using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<Result<IEnumerable<Patient>>> GetAllAsync();
        Task<Result<Patient>> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Patient patient);
        Task<Result<Patient>> UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);
        Task<Result<IEnumerable<Patient>>> GetPatientsByDoctorIdAsync(Guid doctorId);
        Task<Result<int>> CountAsync(IEnumerable<Patient> query);
        Task<Result<List<Patient>>> GetPaginatedAsync(IEnumerable<Patient> query, int page, int pageSize);
    }
}
