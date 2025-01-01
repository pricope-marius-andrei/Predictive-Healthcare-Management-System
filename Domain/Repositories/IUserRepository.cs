using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<Guid>> Register(User user, CancellationToken cancellationToken);
        Task<Result<string>> Login(User user);
        Task<Result<string>> SendPasswordResetEmailAsync(string email, string resetLink);

        Task<Result<User>> ResetPassword(string email, string newPassword, string token);

    }
}
