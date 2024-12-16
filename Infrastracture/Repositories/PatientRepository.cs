using Domain.Common;
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
            _context = context;
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            var patient = await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == id);

            return patient == null ? throw new KeyNotFoundException("Patient not found.") : patient;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .ToListAsync();
        }

        public async Task<Result<Patient>> UpdateAsync(Patient patient)
        {
            ArgumentNullException.ThrowIfNull(patient);
            try
            {
                var existingPatient = await _context.Patients.FindAsync(patient.Id) ?? throw new KeyNotFoundException("Patient not found.");
                _context.Entry(existingPatient).CurrentValues.SetValues(patient);
                await _context.SaveChangesAsync();

                var newPatient = await _context.Patients
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .FirstOrDefaultAsync(p => p.Id == existingPatient.Id);

                if (newPatient == null)
                {
                    throw new KeyNotFoundException("Patient not found.");
                }

                return Result<Patient>.Success(newPatient);
            }
            catch (Exception ex)
            {
                return Result<Patient>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Patients
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<int> CountAsync(IEnumerable<Patient> patients)
        {
            int count = patients.Count();
            return await Task.FromResult(count);
        }

        public async Task<List<Patient>> GetPaginatedAsync(IEnumerable<Patient> patients, int page, int pageSize)
        {
            var pagedPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(pagedPatients);
        }
    }
}