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

        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                throw new KeyNotFoundException(DoctorNotFound);
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
                var existingDoctor = await _context.Doctors.FindAsync(doctor.Id);
                if (existingDoctor == null)
                {
                    throw new KeyNotFoundException(DoctorNotFound);
                }

                doctor.DateOfRegistration = doctor.DateOfRegistration.ToUniversalTime();

                _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                await _context.SaveChangesAsync();

                var newDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Id == existingDoctor.Id);

                if (newDoctor == null)
                {
                    throw new KeyNotFoundException(DoctorNotFound);
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
                throw new KeyNotFoundException(DoctorNotFound);
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
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
