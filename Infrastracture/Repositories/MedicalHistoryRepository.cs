using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private const string MedicalHistoryNotFound = "Medical history not found.";
        private readonly ApplicationDbContext _context;

        public MedicalHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<MedicalHistory>>> GetAllAsync()
        {
            try
            {
                var medicalHistories = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .ToListAsync();
                return Result<IEnumerable<MedicalHistory>>.Success(medicalHistories);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MedicalHistory>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<MedicalHistory>> GetByIdAsync(Guid id)
        {
            try
            {
                var medicalHistory = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .FirstOrDefaultAsync(mh => mh.HistoryId == id);

                if (medicalHistory == null)
                {
                    return Result<MedicalHistory>.Failure(MedicalHistoryNotFound);
                }

                return Result<MedicalHistory>.Success(medicalHistory);
            }
            catch (Exception ex)
            {
                return Result<MedicalHistory>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<IEnumerable<MedicalHistory>>> GetByPatientIdAsync(Guid patientId)
        {
            try
            {
                var medicalHistories = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .Where(mh => mh.PatientId == patientId)
                    .ToListAsync();
                return Result<IEnumerable<MedicalHistory>>.Success(medicalHistories);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<MedicalHistory>>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Guid>> AddAsync(MedicalHistory medicalHistory)
        {
            ArgumentNullException.ThrowIfNull(medicalHistory);
            try
            {
                await _context.MedicalHistories.AddAsync(medicalHistory);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(medicalHistory.HistoryId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<MedicalHistory>> UpdateAsync(MedicalHistory medicalHistory)
        {
            ArgumentNullException.ThrowIfNull(medicalHistory);
            try
            {
                var existingMedicalHistory = await _context.MedicalHistories.FindAsync(medicalHistory.HistoryId);
                if (existingMedicalHistory == null)
                {
                    return Result<MedicalHistory>.Failure(MedicalHistoryNotFound);
                }

                _context.Entry(existingMedicalHistory).CurrentValues.SetValues(medicalHistory);
                await _context.SaveChangesAsync();

                var newMedicalHistory = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .FirstOrDefaultAsync(mh => mh.HistoryId == existingMedicalHistory.HistoryId);

                if (newMedicalHistory == null)
                {
                    return Result<MedicalHistory>.Failure(MedicalHistoryNotFound);
                }

                return Result<MedicalHistory>.Success(newMedicalHistory);
            }
            catch (Exception ex)
            {
                return Result<MedicalHistory>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory == null)
            {
                throw new KeyNotFoundException(MedicalHistoryNotFound);
            }

            _context.MedicalHistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();
        }
    }
}


