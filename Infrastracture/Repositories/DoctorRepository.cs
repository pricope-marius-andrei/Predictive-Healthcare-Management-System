using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        public Task<IEnumerable<Doctor>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Doctor> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> AddAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
