using Application.UseCases.CommandHandlers.DoctorCommandHandlers;
using Application.UseCases.Commands.DoctorCommands;
using Application.Utils;
using AutoMapper;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using FluentValidation;
using NSubstitute;
using Infrastructure.Repositories;

namespace Predictive_Healthcare_Management_System.IntegrationTests
{
    public class CreateDoctorCommandHandlerTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IValidator<CreateDoctorCommand> _validator;
        private readonly ApplicationDbContext _context;
        private readonly CreateDoctorCommandHandler _handler;

        public CreateDoctorCommandHandlerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);

            _doctorRepository = new DoctorRepository(_context);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _validator = Substitute.For<IValidator<CreateDoctorCommand>>();
            _validator.ValidateAsync(Arg.Any<CreateDoctorCommand>(), Arg.Any<CancellationToken>())
                      .Returns(new FluentValidation.Results.ValidationResult());

            _handler = new CreateDoctorCommandHandler(_doctorRepository, _mapper, _validator);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task Handle_ShouldCreateDoctor_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                Username = "doctortest",
                Email = "doctor@example.com",
                Password = "SecurePassword",
                FirstName = "Test",
                LastName = "Doctor",
                PhoneNumber = "1234567890",
                Specialization = "Dermatology"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.IsType<Guid>(result.Data);

            var createdDoctorId = result.Data;

            // Verify that the doctor was added to the database
            var doctor = await _context.Doctors.FindAsync(createdDoctorId);
            Assert.NotNull(doctor);
            Assert.Equal(command.Username, doctor.Username);
            Assert.Equal(command.Email, doctor.Email);
            Assert.Equal(command.FirstName, doctor.FirstName);
            Assert.Equal(command.LastName, doctor.LastName);
            Assert.Equal(command.PhoneNumber, doctor.PhoneNumber);
            Assert.Equal(command.Specialization, doctor.Specialization);
        }
    }
}