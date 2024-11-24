using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IMedicalHistoryRepository
	{
		Task<IEnumerable<MedicalHistory>> GetAllAsync();
		Task<MedicalHistory> GetByIdAsync(Guid id);
		Task<IEnumerable<MedicalHistory>> GetByPatientIdAsync(Guid patientId);
		Task<Result<Guid>> AddAsync(MedicalHistory medicalHistory);
		Task<Result<MedicalHistory>> UpdateAsync(MedicalHistory medicalHistory);
		Task DeleteAsync(Guid id);
	}
}