using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class DoctorRepository(ApplicationDbContext context) : IDoctorRepository
    {
		private const string DoctorNotFound = "Doctor not found.";

        public async Task<Result<Guid>> AddAsync(Doctor doctor)
		{
			ArgumentNullException.ThrowIfNull(doctor);
			try
			{
				await context.Doctors.AddAsync(doctor);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(doctor.PersonId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Doctor> GetByIdAsync(Guid id)
		{
			var doctor = await context.Doctors
				.FirstOrDefaultAsync(d => d.PersonId == id);

			if (doctor == null)
			{
				throw new KeyNotFoundException(DoctorNotFound);
			}

			return doctor;
		}

		public async Task<IEnumerable<Doctor>> GetAllAsync()
		{
			return await context.Doctors.ToListAsync();
		}

		public async Task<Result<Doctor>> UpdateAsync(Doctor doctor)
		{
			ArgumentNullException.ThrowIfNull(doctor);
			try
			{
				var existingDoctor = await context.Doctors.FindAsync(doctor.PersonId);
				if (existingDoctor == null)
				{
					throw new KeyNotFoundException(DoctorNotFound);
				}

				existingDoctor.DateOfRegistration = existingDoctor.DateOfRegistration.ToUniversalTime();

				context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
				await context.SaveChangesAsync();

				var updatedDoctor = await context.Doctors
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
			var doctor = await context.Doctors.FindAsync(id);
			if (doctor == null)
			{
				throw new KeyNotFoundException(DoctorNotFound);
			}

			context.Doctors.Remove(doctor);
			await context.SaveChangesAsync();
		}
	}
}