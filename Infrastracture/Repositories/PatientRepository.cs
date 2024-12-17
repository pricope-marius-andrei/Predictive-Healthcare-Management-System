using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private const string PatientNotFound = "Patient not found.";
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> AddAsync(Patient patient)
        {
            ArgumentNullException.ThrowIfNull(patient);
            try
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(patient.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Patient>> GetByIdAsync(Guid id)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                {
                    return Result<Patient>.Failure(PatientNotFound);
                }

                return Result<Patient>.Success(patient);
            }
            catch (Exception ex)
            {
                return Result<Patient>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Patient>>> GetAllAsync()
        {
            try
            {
                var patients = await _context.Patients
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .ToListAsync();
                return Result<IEnumerable<Patient>>.Success(patients);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Patient>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Patient>> UpdateAsync(Patient patient)
        {
            ArgumentNullException.ThrowIfNull(patient);
            try
            {
                var existingPatient = await _context.Patients.FindAsync(patient.Id);
                if (existingPatient == null)
                {
                    return Result<Patient>.Failure(PatientNotFound);
                }

                _context.Entry(existingPatient).CurrentValues.SetValues(patient);
                await _context.SaveChangesAsync();

                var newPatient = await _context.Patients
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .FirstOrDefaultAsync(p => p.Id == existingPatient.Id);

                if (newPatient == null)
                {
                    return Result<Patient>.Failure(PatientNotFound);
                }

                return Result<Patient>.Success(newPatient);
            }
            catch (Exception ex)
            {
                return Result<Patient>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException(PatientNotFound);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
        public async Task<Result<IEnumerable<Patient>>> GetPatientsByUsernameFilterAsync(string username)
        {
            try
            {
                var patients = await _context.Patients
                    .AsNoTracking()
                    .Include(p => p.MedicalHistories)
                    .Include(p => p.MedicalRecords)
                    .Where(p => p.Username.Contains(username))
                    .ToListAsync();
                return Result<IEnumerable<Patient>>.Success(patients);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Patient>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Patient>>> GetPatientsByDoctorIdAsync(Guid doctorId)
        {
            try
            {
                var patients = await _context.Patients
                    .Where(p => p.DoctorId == doctorId)
                    .ToListAsync();
                return Result<IEnumerable<Patient>>.Success(patients);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Patient>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<int>> CountAsync(IEnumerable<Patient> patients)
        {
            try
            {
                int count = patients.Count();
                return await Task.FromResult(Result<int>.Success(count));
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<List<Patient>>> GetPaginatedAsync(IEnumerable<Patient> patients, int page, int pageSize)
        {
            try
            {
                var pagedPatients = patients
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return await Task.FromResult(Result<List<Patient>>.Success(pagedPatients));
            }
            catch (Exception ex)
            {
                return Result<List<Patient>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }
    }
}
