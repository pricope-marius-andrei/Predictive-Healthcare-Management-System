using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class MedicalHistoryRepository(ApplicationDbContext context) : IMedicalHistoryRepository
    {
        public async Task<IEnumerable<MedicalHistory>> GetAllAsync()
		{
			return await context.MedicalHistories
				.Include(mh => mh.Patient)
				.ToListAsync();
		}

		public async Task<MedicalHistory> GetByIdAsync(Guid id)
		{
			var medicalHistory = await context.MedicalHistories
				.Include(mh => mh.Patient)
				.FirstOrDefaultAsync(mh => mh.HistoryId == id);

			if (medicalHistory == null)
			{
				throw new KeyNotFoundException("Medical history not found.");
			}

			return medicalHistory;
		}

		public async Task<IEnumerable<MedicalHistory>> GetByPatientIdAsync(Guid patientId)
		{
			return await context.MedicalHistories
				.Where(mh => mh.PatientId == patientId)
				.Include(mh => mh.Patient)
				.ToListAsync();
		}

		public async Task<Result<Guid>> AddAsync(MedicalHistory medicalHistory)
		{
			ArgumentNullException.ThrowIfNull(medicalHistory);
			try
			{
				await context.MedicalHistories.AddAsync(medicalHistory);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(medicalHistory.HistoryId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Result<MedicalHistory>> UpdateAsync(MedicalHistory medicalHistory)
		{
			ArgumentNullException.ThrowIfNull(medicalHistory);
			try
			{
				var existingMedicalHistory = await context.MedicalHistories.FindAsync(medicalHistory.HistoryId);
				if (existingMedicalHistory == null)
				{
					throw new KeyNotFoundException("Medical history not found.");
				}

				context.Entry(existingMedicalHistory).CurrentValues.SetValues(medicalHistory);
				await context.SaveChangesAsync();

				var updatedMedicalHistory = await context.MedicalHistories
					.Include(mh => mh.Patient)
					.FirstOrDefaultAsync(mh => mh.HistoryId == existingMedicalHistory.HistoryId);

				if (updatedMedicalHistory == null)
				{
					throw new KeyNotFoundException("Medical history not found.");
				}

				return Result<MedicalHistory>.Success(updatedMedicalHistory);
			}
			catch (Exception ex)
			{
				return Result<MedicalHistory>.Failure(ex.Message);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			var medicalHistory = await context.MedicalHistories.FindAsync(id);
			if (medicalHistory == null)
			{
				throw new KeyNotFoundException("Medical history not found.");
			}

			context.MedicalHistories.Remove(medicalHistory);
			await context.SaveChangesAsync();
		}
	}
}