using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
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
			ArgumentNullException.ThrowIfNull(patient);
			try
			{
				await _context.Patients.AddAsync(patient);
				await _context.SaveChangesAsync();
				return Result<Guid>.Success(patient.PersonId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Patient> GetByIdAsync(Guid id)
		{
			var patient = await _context.Patients
				.Include(p => p.MedicalHistories)
				.Include(p => p.MedicalRecords)
				.FirstOrDefaultAsync(p => p.PersonId == id);

			return patient ?? throw new KeyNotFoundException("Patient not found.");
		}

		public async Task<IEnumerable<Patient>> GetAllAsync()
		{
			return await _context.Patients
				.Include(p => p.MedicalHistories)
				.Include(p => p.MedicalRecords)
				.ToListAsync();
		}

		public async Task<Result<Patient>> UpdateAsync(Patient patient)
		{
			ArgumentNullException.ThrowIfNull(patient);
			try
			{
				var existingPatient = await _context.Patients.FindAsync(patient.PersonId)
									   ?? throw new KeyNotFoundException("Patient not found.");
				_context.Entry(existingPatient).CurrentValues.SetValues(patient);
				await _context.SaveChangesAsync();

				var updatedPatient = await _context.Patients
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
