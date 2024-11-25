using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class PatientRepository(ApplicationDbContext context) : IPatientRepository
    {
        public async Task<Result<Guid>> AddAsync(Patient patient)
		{
			ArgumentNullException.ThrowIfNull(patient);
			try
			{
				await context.Patients.AddAsync(patient);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(patient.PersonId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Patient> GetByIdAsync(Guid id)
		{
			var patient = await context.Patients
				.Include(p => p.MedicalHistories)
				.Include(p => p.MedicalRecords)
				.FirstOrDefaultAsync(p => p.PersonId == id);

			return patient ?? throw new KeyNotFoundException("Patient not found.");
		}

		public async Task<IEnumerable<Patient>> GetAllAsync()
		{
			return await context.Patients
				.Include(p => p.MedicalHistories)
				.Include(p => p.MedicalRecords)
				.ToListAsync();
		}

		public async Task<Result<Patient>> UpdateAsync(Patient patient)
		{
			ArgumentNullException.ThrowIfNull(patient);
			try
			{
				var existingPatient = await context.Patients.FindAsync(patient.PersonId)
									   ?? throw new KeyNotFoundException("Patient not found.");
				context.Entry(existingPatient).CurrentValues.SetValues(patient);
				await context.SaveChangesAsync();

				var updatedPatient = await context.Patients
					.Include(p => p.MedicalHistories)
					.Include(p => p.MedicalRecords)
					.FirstOrDefaultAsync(p => p.PersonId == existingPatient.PersonId);

				if (updatedPatient == null)
				{
					throw new KeyNotFoundException("Patient not found.");
				}

				return Result<Patient>.Success(updatedPatient);
			}
			catch (Exception ex)
			{
				return Result<Patient>.Failure(ex.Message);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			var patient = await context.Patients.FindAsync(id);
			if (patient == null)
			{
				throw new KeyNotFoundException("Patient not found.");
			}

			context.Patients.Remove(patient);
			await context.SaveChangesAsync();
		}
	}
}