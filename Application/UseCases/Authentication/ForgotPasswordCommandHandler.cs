using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string?> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            string resetToken = _tokenGenerator.GenerateToken();
            string resetLink = request.ClientUrl + $"?token={resetToken}";

            var result = await _userRepository.SendPasswordResetEmailAsync(request.Email, resetLink);

            if (result.IsSuccess)
            {
                return resetToken;
            }
            else
            {
                return null;
            }
        }
    }
}
