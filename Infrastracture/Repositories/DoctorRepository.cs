using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private const string _doctorNotFoundMessage = "Doctor not found.";
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
                return Result<Guid>.Failure(ex.Message);
            }
        }
        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            var doctor = await _context.Doctors // include doctor's related entities records and other stuff 
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
            {
                throw new KeyNotFoundException(_doctorNotFoundMessage);
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
                var existingDoctor = await _context.Doctors.FindAsync(doctor.DoctorId) ?? throw new KeyNotFoundException(_doctorNotFoundMessage);
                doctor.DateOfRegistration = doctor.DateOfRegistration.ToUniversalTime();

                _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                await _context.SaveChangesAsync();

                var newDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId == existingDoctor.DoctorId);

                return newDoctor == null ? throw new KeyNotFoundException(_doctorNotFoundMessage) : Result<Doctor>.Success(newDoctor);
            }
            catch (Exception ex)
            {
                return Result<Doctor>.Failure(ex.InnerException!.ToString());
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            Doctor? doctor = await _context.Doctors.FindAsync(id) ?? throw new KeyNotFoundException(_doctorNotFoundMessage);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
