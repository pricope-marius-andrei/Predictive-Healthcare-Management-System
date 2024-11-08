using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
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
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(patient.PatientId);
            }
            catch(Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
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

        public async Task UpdateAsync(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));

            var existingPatient = await _context.Patients.FindAsync(patient.PatientId);
            if (existingPatient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            existingPatient.Username = patient.Username;
            existingPatient.Email = patient.Email;
            existingPatient.Password = patient.Password;
            existingPatient.FirstName = patient.FirstName;
            existingPatient.LastName = patient.LastName;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Address = patient.Address;
            existingPatient.Gender = patient.Gender;
            existingPatient.Height = patient.Height;
            existingPatient.Weight = patient.Weight;

            _context.Patients.Update(existingPatient);

            await _context.SaveChangesAsync();
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