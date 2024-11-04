using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<IEnumerable<MedicalRecord>> GetAllAsync();
        Task<MedicalRecord> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(MedicalRecord medicalRecord);
        Task UpdateAsync(MedicalRecord medicalRecord);
        Task DeleteAsync(Guid id);
    }
}