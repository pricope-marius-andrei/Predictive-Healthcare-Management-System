using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> AddAsync(Doctor doctor)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
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
                throw new KeyNotFoundException("Doctor not found.");
            }
            return doctor;
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }
        public Task UpdateAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
