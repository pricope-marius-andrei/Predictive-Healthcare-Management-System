using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private const string Message = "Medical history not found.";
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

            return medicalHistory == null ? throw new KeyNotFoundException(Message) : medicalHistory;
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
            ArgumentNullException.ThrowIfNull(medicalHistory);
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
            ArgumentNullException.ThrowIfNull(medicalHistory);
            try
            {
                var existingMedicalHistory = await _context.MedicalHistories.FindAsync(medicalHistory.HistoryId);
                if (existingMedicalHistory == null)
                {
                    throw new KeyNotFoundException(Message
                        );
                }

                _context.Entry(existingMedicalHistory).CurrentValues.SetValues(medicalHistory);
                await _context.SaveChangesAsync();

                var newMedicalHistory = await _context.MedicalHistories
                    .Include(mh => mh.Patient)
                    .FirstOrDefaultAsync(mh => mh.HistoryId == existingMedicalHistory.HistoryId) ?? throw new KeyNotFoundException(Message);
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
                throw new KeyNotFoundException(Message);
            }

            _context.MedicalHistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();
        }
    }
}
