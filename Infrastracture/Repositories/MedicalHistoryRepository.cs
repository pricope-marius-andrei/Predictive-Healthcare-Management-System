using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public MedicalHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicalHistory>> GetAllAsync()
        {
            return await _context.MedicalHistories
                .Include(mh => mh.Patient)
                .ToListAsync();
        }

        public async Task<MedicalHistory> GetByIdAsync(Guid id)
        {
            var medicalHistory = await _context.MedicalHistories
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
            return await _context.MedicalHistories
                .Include(mh => mh.Patient)
                .Where(mh => mh.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Result<Guid>> AddAsync(MedicalHistory medicalHistory)
        {
            if (medicalHistory == null) throw new ArgumentNullException(nameof(medicalHistory));
            try
            {
                await _context.MedicalHistories.AddAsync(medicalHistory);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(medicalHistory.HistoryId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException.ToString());
            }
        }

        public async Task<Result<MedicalHistory>> UpdateAsync(MedicalHistory medicalHistory)
        {
            if (medicalHistory == null) throw new ArgumentNullException(nameof(medicalHistory));
            try
            {
                var existingMedicalHistory = await _context.MedicalHistories.FindAsync(medicalHistory.HistoryId);
                if (existingMedicalHistory == null)
                {
                    throw new KeyNotFoundException("Medical history not found.");
                }

                _context.Entry(existingMedicalHistory).CurrentValues.SetValues(medicalHistory);
                await _context.SaveChangesAsync();

                var newMedicalHistory = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .FirstOrDefaultAsync(mh => mh.HistoryId == existingMedicalHistory.HistoryId);

                if (newMedicalHistory == null)
                {
                    throw new KeyNotFoundException("Medical history not found.");
                }

                return Result<MedicalHistory>.Success(newMedicalHistory);
            }
            catch (Exception ex)
            {
                return Result<MedicalHistory>.Failure(ex.InnerException.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory == null)
            {
                throw new KeyNotFoundException("Medical history not found.");
            }

            _context.MedicalHistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();
        }
    }
}
