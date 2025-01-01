using Domain.Common;

namespace Domain.Repositories
{
    public interface IEmailService
    {
        Task<Result<string>> SendEmailAsync(string to, string subject, string body);
    }
}
