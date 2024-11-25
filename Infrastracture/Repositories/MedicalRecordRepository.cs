using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class MedicalRecordRepository(ApplicationDbContext context) : IMedicalRecordRepository
    {
        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
		{
			return await context.MedicalRecords
				.Include(mr => mr.Patient)
				.Include(mr => mr.Doctor)
				.ToListAsync();
		}

		public async Task<MedicalRecord> GetByIdAsync(Guid id)
		{
			var medicalRecord = await context.MedicalRecords
				.Include(mr => mr.Patient)
				.Include(mr => mr.Doctor)
				.FirstOrDefaultAsync(mr => mr.RecordId == id);

			if (medicalRecord == null)
			{
				throw new KeyNotFoundException("Medical record not found.");
			}

			return medicalRecord;
		}

		public async Task<IEnumerable<MedicalRecord>> GetByPatientIdAsync(Guid patientId)
		{
			return await context.MedicalRecords
				.Where(mr => mr.PatientId == patientId)
				.Include(mr => mr.Doctor)
				.ToListAsync();
		}

		public async Task<Result<Guid>> AddAsync(MedicalRecord medicalRecord)
		{
			ArgumentNullException.ThrowIfNull(medicalRecord);
			try
			{
				await context.MedicalRecords.AddAsync(medicalRecord);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(medicalRecord.RecordId);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure(ex.Message);
			}
		}

		public async Task<Result<MedicalRecord>> UpdateAsync(MedicalRecord medicalRecord)
		{
			ArgumentNullException.ThrowIfNull(medicalRecord);
			try
			{
				var existingMedicalRecord = await context.MedicalRecords.FindAsync(medicalRecord.RecordId);
				if (existingMedicalRecord == null)
				{
					throw new KeyNotFoundException("Medical record not found.");
				}

				context.Entry(existingMedicalRecord).CurrentValues.SetValues(medicalRecord);
				await context.SaveChangesAsync();

				var updatedMedicalRecord = await context.MedicalRecords
					.Include(mr => mr.Patient)
					.Include(mr => mr.Doctor)
					.FirstOrDefaultAsync(mr => mr.RecordId == existingMedicalRecord.RecordId);

				if (updatedMedicalRecord == null)
				{
					throw new KeyNotFoundException("Medical record not found.");
				}

				return Result<MedicalRecord>.Success(updatedMedicalRecord);
			}
			catch (Exception ex)
			{
				return Result<MedicalRecord>.Failure(ex.Message);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			var medicalRecord = await context.MedicalRecords.FindAsync(id);
			if (medicalRecord == null)
			{
				throw new KeyNotFoundException("Medical record not found.");
			}

			context.MedicalRecords.Remove(medicalRecord);
			await context.SaveChangesAsync();
		}
	}
}