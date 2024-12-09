using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(mr => mr.DoctorId);

            modelBuilder.Entity<MedicalHistory>()
                .HasOne(mh => mh.Patient)
                .WithMany(p => p.MedicalHistories)
                .HasForeignKey(mh => mh.PatientId);

            modelBuilder.Entity<Patient>()
                .Property(p => p.FirstName)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.LastName)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Gender)
                .IsRequired();

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                 v => v.ToUniversalTime(),
                 v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.Entity<Doctor>()
                .Property(d => d.DateOfRegistration)
                .HasConversion(dateTimeConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}