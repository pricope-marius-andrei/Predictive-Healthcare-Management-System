using Application.UseCases.CommandHandlers.DoctorCommandHandlers;
using Application.UseCases.Commands.DoctorCommands;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
	public class DeleteDoctorCommandHandlerTest
	{
		private readonly IDoctorRepository _mockRepository;

		public DeleteDoctorCommandHandlerTest()
		{
			_mockRepository = Substitute.For<IDoctorRepository>();
		}

		[Fact]
		public async Task Handle_ShouldDeleteDoctor_WhenDoctorExists()
		{
			// Arrange
			var personId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf");
			var doctor = new Doctor
			{
				PersonId = personId,
				Username = "doctorUsername",
				Email = "doctor@example.com",
				Password = "password",
				FirstName = "John",
				LastName = "Doe",
				PhoneNumber = "1234567890",
				Specialization = "Cardiology",
				DateOfRegistration = DateTime.UtcNow
			};

			_mockRepository.GetByIdAsync(personId).Returns(doctor);
			var command = new DeleteDoctorCommand { PersonId = personId };

			var handler = new DeleteDoctorCommandHandler(_mockRepository);

			// Act
			await handler.Handle(command, CancellationToken.None);

			// Assert
			await _mockRepository.Received(1).DeleteAsync(personId);
		}

		[Fact]
		public async Task Handle_ShouldThrowInvalidOperationException_WhenDoctorDoesNotExist()
		{
			// Arrange
			var personId = Guid.Parse("d7257654-ac75-4633-bdd4-fabea28387cf");
			_mockRepository.GetByIdAsync(personId).Returns((Doctor?)null);

			var command = new DeleteDoctorCommand { PersonId = personId };
			var handler = new DeleteDoctorCommandHandler(_mockRepository);

			// Act & Assert
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
			Assert.Equal("Doctor not found.", exception.Message);
		}
	}
}