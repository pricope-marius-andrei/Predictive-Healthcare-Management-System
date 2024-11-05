using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            this._context = _context;
        }
        public async Task<Guid> AddAsync(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient.PatientId;
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            return await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.PatientId == id);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .ToListAsync();
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