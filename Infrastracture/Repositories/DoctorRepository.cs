using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private const string DoctorNotFound = "Doctor not found.";
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
                return Result<Guid>.Success(doctor.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Doctor>> GetByIdAsync(Guid id)
        {
            try
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                {
                    return Result<Doctor>.Failure(DoctorNotFound);
                }

                return Result<Doctor>.Success(doctor);
            }
            catch (Exception ex)
            {
                return Result<Doctor>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<IEnumerable<Doctor>>> GetAllAsync()
        {
            try
            {
                var doctors = await _context.Doctors.ToListAsync();
                return Result<IEnumerable<Doctor>>.Success(doctors);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Doctor>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Doctor>> UpdateAsync(Doctor doctor)
        {
            ArgumentNullException.ThrowIfNull(doctor);
            try
            {
                var existingDoctor = await _context.Doctors.FindAsync(doctor.Id);
                if (existingDoctor == null)
                {
                    return Result<Doctor>.Failure(DoctorNotFound);
                }

                doctor.DateOfRegistration = doctor.DateOfRegistration.ToUniversalTime();

                foreach (var property in typeof(Doctor).GetProperties())
                {
                    var newValue = property.GetValue(doctor);
                    if (newValue != null)
                    {
                        property.SetValue(existingDoctor, newValue);
                    }
                }

                await _context.SaveChangesAsync();

                var newDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Id == existingDoctor.Id);

                if (newDoctor == null)
                {
                    return Result<Doctor>.Failure(DoctorNotFound);
                }

                return Result<Doctor>.Success(newDoctor);
            }
            catch (Exception ex)
            {
                return Result<Doctor>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException(DoctorNotFound);
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<Result<IEnumerable<Doctor>>> GetDoctorsByUsernameFilterAsync(string username)
        {
            try
            {
                var doctors = await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.MedicalRecords)
                    .Where(d => d.Username.Contains(username))
                    .ToListAsync();
                return Result<IEnumerable<Doctor>>.Success(doctors);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Doctor>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<int> CountAsync(IEnumerable<Doctor> doctors)
        {
            int count = doctors.Count();
            return await Task.FromResult(count);
        }

        public async Task<List<Doctor>> GetPaginatedAsync(IEnumerable<Doctor> doctors, int page, int pageSize)
        {
            var pagedDoctors = doctors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(pagedDoctors);
        }
    }
}


