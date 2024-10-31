using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<Test>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .ValueGeneratedOnAdd();
                    entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                    entity.ToTable("tests");

                }

                );
        }
    }
}
