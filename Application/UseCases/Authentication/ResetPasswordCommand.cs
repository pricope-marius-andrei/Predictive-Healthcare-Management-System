using Domain.Common;
using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Authentication
{
    public class ResetPasswordCommand : IRequest<Result<User>>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
