using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentication
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<User>>
    {
        private readonly IUserRepository _userRepository;
        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<User>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Password != request.ConfirmPassword)
                    throw new ArgumentException("Passwords do not match.");
            
                return await _userRepository.ResetPassword(request.Email, request.Password, request.Token);

            }
            catch(Exception ex)
            {
                return Result<User>.Failure(ex.Message);
            }
            
        }
    }
}
