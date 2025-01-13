using Application.UseCases.Authentication;
using Domain.Common;
using Domain.Repositories;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class ForgotPasswordCommandHandlerTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly ITokenGenerator _tokenGeneratorMock;
        private readonly ForgotPasswordCommandHandler _handler;

        public ForgotPasswordCommandHandlerTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _tokenGeneratorMock = Substitute.For<ITokenGenerator>();
            _handler = new ForgotPasswordCommandHandler(_userRepositoryMock, _tokenGeneratorMock);
        }

        [Fact]
        public async Task Handle_ShouldReturnResetToken_WhenEmailSentSuccessfully()
        {
            // Arrange
            var command = new ForgotPasswordCommand
            {
                Email = "test@example.com",
                ClientUrl = "http://example.com/reset-password"
            };
            var resetToken = "some-token";
            var resetLink = $"{command.ClientUrl}?token={resetToken}";
            _tokenGeneratorMock.GenerateToken().Returns(resetToken);
            _userRepositoryMock.SendPasswordResetEmailAsync(command.Email, resetLink)
                .Returns(Result<string>.Success("Email sent"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(resetToken, result);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEmailSendingFails()
        {
            // Arrange
            var command = new ForgotPasswordCommand
            {
                Email = "test@example.com",
                ClientUrl = "http://example.com/reset-password"
            };
            var resetToken = "some-token";
            var resetLink = $"{command.ClientUrl}?token={resetToken}";
            _tokenGeneratorMock.GenerateToken().Returns(resetToken);
            _userRepositoryMock.SendPasswordResetEmailAsync(command.Email, resetLink)
                .Returns(Result<string>.Failure("Failed to send email"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
