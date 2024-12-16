using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalRecordRepository
    {
        Task<Result<IEnumerable<MedicalRecord>>> GetAllAsync();
        Task<Result<MedicalRecord>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<MedicalRecord>>> GetByPatientIdAsync(Guid patientId);
        Task<Result<Guid>> AddAsync(MedicalRecord medicalRecord);
        Task<Result<MedicalRecord>> UpdateAsync(MedicalRecord medicalRecord);
        Task DeleteAsync(Guid id);
    }
}
