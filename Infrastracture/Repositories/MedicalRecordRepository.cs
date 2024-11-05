using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        public Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MedicalRecord> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(MedicalRecord medicalRecord)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MedicalRecord medicalRecord)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
