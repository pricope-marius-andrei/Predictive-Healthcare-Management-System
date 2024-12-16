using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private const string MedicalRecordNotFound = "Medical record not found.";
        private readonly ApplicationDbContext _context;

        public MedicalRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<MedicalRecord>>> GetAllAsync()
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .ToListAsync();
                return Result<IEnumerable<MedicalRecord>>.Success(medicalRecords);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MedicalRecord>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<MedicalRecord>> GetByIdAsync(Guid id)
        {
            try
            {
                var medicalRecord = await _context.MedicalRecords
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .FirstOrDefaultAsync(mr => mr.RecordId == id);

                if (medicalRecord == null)
                {
                    return Result<MedicalRecord>.Failure(MedicalRecordNotFound);
                }

                return Result<MedicalRecord>.Success(medicalRecord);
            }
            catch (Exception ex)
            {
                return Result<MedicalRecord>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<IEnumerable<MedicalRecord>>> GetByPatientIdAsync(Guid patientId)
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords
                    .Where(mr => mr.PatientId == patientId)
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .ToListAsync();
                return Result<IEnumerable<MedicalRecord>>.Success(medicalRecords);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MedicalRecord>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Guid>> AddAsync(MedicalRecord medicalRecord)
        {
            ArgumentNullException.ThrowIfNull(medicalRecord);
            try
            {
                await _context.MedicalRecords.AddAsync(medicalRecord);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(medicalRecord.RecordId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<MedicalRecord>> UpdateAsync(MedicalRecord medicalRecord)
        {
            ArgumentNullException.ThrowIfNull(medicalRecord);
            try
            {
                var existingMedicalRecord = await _context.MedicalRecords.FindAsync(medicalRecord.RecordId);
                if (existingMedicalRecord == null)
                {
                    return Result<MedicalRecord>.Failure(MedicalRecordNotFound);
                }

                _context.Entry(existingMedicalRecord).CurrentValues.SetValues(medicalRecord);
                await _context.SaveChangesAsync();

                var newMedicalRecord = await _context.MedicalRecords
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .FirstOrDefaultAsync(mr => mr.RecordId == existingMedicalRecord.RecordId);

                if (newMedicalRecord == null)
                {
                    return Result<MedicalRecord>.Failure(MedicalRecordNotFound);
                }

                return Result<MedicalRecord>.Success(newMedicalRecord);
            }
            catch (Exception ex)
            {
                return Result<MedicalRecord>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                throw new KeyNotFoundException(MedicalRecordNotFound);
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
        }
    }
}

