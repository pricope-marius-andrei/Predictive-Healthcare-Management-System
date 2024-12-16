using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalHistoryRepository
    {
        Task<Result<IEnumerable<MedicalHistory>>> GetAllAsync();
        Task<Result<MedicalHistory>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<MedicalHistory>>> GetByPatientIdAsync(Guid patientId);
        Task<Result<Guid>> AddAsync(MedicalHistory medicalHistory);
        Task<Result<MedicalHistory>> UpdateAsync(MedicalHistory medicalHistory);
        Task DeleteAsync(Guid id);
    }
}
