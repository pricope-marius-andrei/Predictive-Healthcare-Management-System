using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        public Task<IEnumerable<MedicalHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MedicalHistory> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(MedicalHistory medicalHistory)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MedicalHistory medicalHistory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
