using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .ToListAsync();
        }

        public async Task<MedicalRecord> GetByIdAsync(Guid id)
        {
            var medicalRecord = await _context.MedicalRecords
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
            return await _context.MedicalRecords
                .Where(mr => mr.PatientId == patientId)
                .Include(mr => mr.Patient)
                .Include(mr => mr.Doctor)
                .ToListAsync();
        }

        public async Task<Result<Guid>> AddAsync(MedicalRecord medicalRecord)
        {
            if (medicalRecord == null) throw new ArgumentNullException(nameof(medicalRecord));
            try
            {
                await _context.MedicalRecords.AddAsync(medicalRecord);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(medicalRecord.RecordId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }

        public async Task<Result<MedicalRecord>> UpdateAsync(MedicalRecord medicalRecord)
        {
            if (medicalRecord == null) throw new ArgumentNullException(nameof(medicalRecord));
            try
            {
                var existingMedicalRecord = await _context.MedicalRecords.FindAsync(medicalRecord.RecordId);
                if (existingMedicalRecord == null)
                {
                    throw new KeyNotFoundException("Medical record not found.");
                }

                _context.Entry(existingMedicalRecord).CurrentValues.SetValues(medicalRecord);
                await _context.SaveChangesAsync();

                var newMedicalRecord = await _context.MedicalRecords
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .FirstOrDefaultAsync(mr => mr.RecordId == existingMedicalRecord.RecordId);

                if (newMedicalRecord == null)
                {
                    throw new KeyNotFoundException("Medical record not found.");
                }

                return Result<MedicalRecord>.Success(newMedicalRecord);
            }
            catch (Exception ex)
            {
                return Result<MedicalRecord>.Failure(ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                throw new KeyNotFoundException("Medical record not found.");
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
        }
    }
}

