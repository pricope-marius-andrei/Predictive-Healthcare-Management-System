using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        public Task<IEnumerable<Patient>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Patient> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(Patient patient)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Patient patient)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
