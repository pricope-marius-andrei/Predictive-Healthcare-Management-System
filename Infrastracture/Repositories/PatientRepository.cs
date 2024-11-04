using Domain.Entities;
using Infrastracture.Persistence;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext context;
        public PatientRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public Task<IEnumerable<Patient>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Patient> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> AddAsync(Patient patient)
        {
           await context.Patients.AddAsync(patient);
           await context.SaveChangesAsync();
           return patient.UserId;
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
