using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string?>
    {
        private readonly IUserRepository _userRepository;

        public ForgotPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string?> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            string resetToken = Guid.NewGuid().ToString(); 
            string resetLink = request.ClientUrl + $"?token={resetToken}";

            var result = await _userRepository.SendPasswordResetEmailAsync(request.Email, resetLink);

            if(result.IsSuccess)
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
