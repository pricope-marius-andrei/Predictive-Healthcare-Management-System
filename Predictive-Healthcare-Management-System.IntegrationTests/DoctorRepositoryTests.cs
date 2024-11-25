using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Predictive_Healthcare_Management_System.IntegrationTests;

public class DoctorRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public DoctorRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDoctors()
    {
        // Arrange
        using (var context = new ApplicationDbContext(_options))
        {
            context.Doctors.AddRange(
                new Doctor { PersonId = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Specialization = "Cardiology" },
                new Doctor { PersonId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Specialization = "Neurology" }
            );
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new DoctorRepository(context);

            // Act
            var doctors = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(doctors);
            Assert.Equal(2, doctors.Count());
        }
    }
}