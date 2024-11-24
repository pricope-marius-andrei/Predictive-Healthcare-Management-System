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

		public DbSet<Person> People { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<MedicalRecord> MedicalRecords { get; set; }
		public DbSet<MedicalHistory> MedicalHistories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasPostgresExtension("uuid-ossp");

			// Map Person to "People" table
			modelBuilder.Entity<Person>()
				.ToTable("People");

			// Map Patient to "Patients" table and specify base type
			modelBuilder.Entity<Patient>()
				.ToTable("Patients")
				.HasBaseType<Person>();

			// Map Doctor to "Doctors" table and specify base type
			modelBuilder.Entity<Doctor>()
				.ToTable("Doctors")
				.HasBaseType<Person>();

			modelBuilder.Entity<MedicalRecord>()
				.HasOne(mr => mr.Patient)
				.WithMany(p => p.MedicalRecords)
				.HasForeignKey(mr => mr.PatientId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<MedicalRecord>()
				.HasOne(mr => mr.Doctor)
				.WithMany()
				.HasForeignKey(mr => mr.DoctorId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<MedicalHistory>()
				.HasOne(mh => mh.Patient)
				.WithMany(p => p.MedicalHistories)
				.HasForeignKey(mh => mh.PatientId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Person>()
				.HasIndex(p => p.Username)
				.IsUnique();

			modelBuilder.Entity<Person>()
				.HasIndex(p => p.Email)
				.IsUnique();

			modelBuilder.Entity<Patient>()
				.Property(p => p.DateOfBirth)
				.IsRequired();

			modelBuilder.Entity<Patient>()
				.Property(p => p.Height)
				.IsRequired();

			modelBuilder.Entity<Patient>()
				.Property(p => p.Weight)
				.IsRequired();

			modelBuilder.Entity<Doctor>()
				.Property(d => d.Specialization)
				.IsRequired();

			var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
				v => v.ToUniversalTime(),
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			modelBuilder.Entity<Person>()
				.Property(p => p.DateOfRegistration)
				.HasConversion(dateTimeConverter);

			modelBuilder.Entity<MedicalRecord>()
				.Property(mr => mr.DateOfVisit)
				.HasConversion(dateTimeConverter);

			modelBuilder.Entity<MedicalHistory>()
				.Property(mh => mh.DateOfDiagnosis)
				.HasConversion(dateTimeConverter);

			base.OnModelCreating(modelBuilder);
		}
	}
}