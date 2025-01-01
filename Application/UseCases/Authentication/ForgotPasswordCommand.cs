using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Authentication
{
    public class ForgotPasswordCommand : IRequest<string>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ClientUrl { get; set; }  
    }
}
