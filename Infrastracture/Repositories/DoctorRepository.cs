using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private const string DOCTOR_NOT_FOUND = "Doctor not found.";
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> AddAsync(Doctor doctor)
        {
            ArgumentNullException.ThrowIfNull(doctor);
            try
            {
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(doctor.DoctorId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }
        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            var doctor = await _context.Doctors // include doctor's related entities records and other stuff 
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
            {
                throw new KeyNotFoundException(DOCTOR_NOT_FOUND);
            }
            return doctor;
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }
        public async Task<Result<Doctor>> UpdateAsync(Doctor doctor)
        {
            ArgumentNullException.ThrowIfNull(doctor);
            try
            {
                var existingDoctor = await _context.Doctors.FindAsync(doctor.DoctorId);
                if (existingDoctor == null)
                {
                    throw new KeyNotFoundException(DOCTOR_NOT_FOUND);
                }

                doctor.DateOfRegistration = doctor.DateOfRegistration.ToUniversalTime();

                _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                await _context.SaveChangesAsync();

                var newDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId == existingDoctor.DoctorId);

                if (newDoctor == null)
                {
                    throw new KeyNotFoundException(DOCTOR_NOT_FOUND);
                }

                return Result<Doctor>.Success(newDoctor);
            }
            catch (Exception ex)
            {
                return Result<Doctor>.Failure(ex.InnerException!.ToString());
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException(DOCTOR_NOT_FOUND);
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsByUsernameFilterAsync(string username)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.MedicalRecords)
                .Where(d => d.Username.Contains(username))
                .ToListAsync();
        }
    }
}
