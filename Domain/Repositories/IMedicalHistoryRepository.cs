using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalHistoryRepository
    {
        Task<IEnumerable<MedicalHistory>> GetAllAsync();
        Task<MedicalHistory> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(MedicalHistory medicalHistory);
        Task UpdateAsync(MedicalHistory medicalHistory);
        Task DeleteAsync(Guid id);
    }
}
