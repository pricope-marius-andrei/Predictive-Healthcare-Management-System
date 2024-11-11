using System.Net;
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

        public async Task<Result<Guid>> AddAsync(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            try
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(patient.PatientId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<Patient> GetByIdAsync(Guid id)
        {
            var patient = await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            return patient;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.MedicalHistories)
                .Include(p => p.MedicalRecords)
                .ToListAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            try
            {
                var existingPatient = await _context.Patients.FindAsync(patient.PatientId);
                if (existingPatient == null)
                {
                    throw new KeyNotFoundException("Patient not found.");
                }

                _context.Entry(existingPatient).CurrentValues.SetValues(patient);
                await _context.SaveChangesAsync();

                var newPatient = await _context.Patients
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .FirstOrDefaultAsync(p => p.PatientId == existingPatient.PatientId);

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
    }
}