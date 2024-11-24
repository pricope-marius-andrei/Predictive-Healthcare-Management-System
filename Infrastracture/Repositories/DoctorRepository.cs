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
				return Result<Guid>.Success(doctor.PersonId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Doctor> GetByIdAsync(Guid id)
		{
			var doctor = await _context.Doctors
				.FirstOrDefaultAsync(d => d.PersonId == id);

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
				var existingDoctor = await _context.Doctors.FindAsync(doctor.PersonId);
				if (existingDoctor == null)
				{
					throw new KeyNotFoundException(DoctorNotFound);
				}

				existingDoctor.DateOfRegistration = existingDoctor.DateOfRegistration.ToUniversalTime();

				_context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
				await _context.SaveChangesAsync();

				var updatedDoctor = await _context.Doctors
					.FirstOrDefaultAsync(d => d.PersonId == existingDoctor.PersonId);

				if (updatedDoctor == null)
				{
					throw new KeyNotFoundException(DoctorNotFound);
				}

				return Result<Doctor>.Success(updatedDoctor);
			}
			catch (Exception ex)
			{
				return Result<Doctor>.Failure(ex.Message);
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
	}
}
