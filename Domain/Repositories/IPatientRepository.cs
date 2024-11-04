namespace Domain.Entities
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);
    }
}
