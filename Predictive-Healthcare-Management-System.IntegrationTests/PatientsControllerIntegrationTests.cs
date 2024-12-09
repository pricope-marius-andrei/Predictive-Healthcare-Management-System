/*
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.UseCases.Commands.Patient;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Predictive_Healthcare_Management_System.IntegrationTests
{
    public class PatientsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ApplicationDbContext _dbContext;

        private const string BaseUrl = "/api/v1/patients";

        public PatientsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this._factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing"); // Set the environment to "Testing"

                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration (PostgreSQL)
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add InMemory Database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    // Build the service provider
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

            // Create a scope to get the ApplicationDbContext
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            }
        }

        [Fact]
        public void GivenPatients_WhenGetAllIsCalled_ThenReturnsCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = client.GetAsync(BaseUrl).Result;

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public void GivenExistingPatients_WhenGetAllIsCalled_ThenReturnsCorrectPatients()
        {
            // Arrange
            var client = _factory.CreateClient();
            CreateSUT();

            // Act
            var response = client.GetAsync(BaseUrl).Result;

            // Assert
            response.EnsureSuccessStatusCode();
            var patients = response.Content.ReadAsStringAsync().Result;
            patients.Should().Contain("John Doe");
        }

        [Fact]
        public async Task GivenValidPatient_WhenCreateIsCalled_ThenAddsPatientToDatabase()
        {
            // Arrange
            var client = _factory.CreateClient();

            var command = new CreatePatientCommand
            {
                Username = "johndoe",
                Email = "johndoe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Address = "123 Main Street",
                Gender = "Male",
                Height = 180.0m,
                Weight = 75.0m,
                DateOfBirth = new DateTime(1990, 1, 1),
                DateOfRegistration = DateTime.UtcNow
            };

            // Act
            var postResponse = await client.PostAsJsonAsync(BaseUrl, command);
            postResponse.EnsureSuccessStatusCode();

            // Assert
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Username == "johndoe");
            patient.Should().NotBeNull();
            patient.Email.Should().Be("johndoe@example.com");
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private void CreateSUT()
        {
            var patient = new Patient
            {
                Username = "johndoe",
                Email = "johndoe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                // Address = "123 Main Street",
                Gender = "Male",
                Height = 180.0m,
                Weight = 75.0m,
                DateOfBirth = new DateTime(1990, 1, 1),
                DateOfRegistration = DateTime.UtcNow
            };
            _dbContext.Patients.Add(patient);
            _dbContext.SaveChanges();
        }
    }
}
*/
