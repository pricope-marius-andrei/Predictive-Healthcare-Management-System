using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<IEnumerable<MedicalRecord>> GetAllAsync();
        Task<MedicalRecord> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(Guid patientId);
        Task<Result<Guid>> AddAsync(MedicalRecord medicalRecord);
        Task<Result<MedicalRecord>> UpdateAsync(MedicalRecord medicalRecord);
        Task DeleteAsync(Guid id);
    }
}
