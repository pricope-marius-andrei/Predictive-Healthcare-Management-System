using Application.UseCases.Authentication;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
    

        public AuthController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var userId = await _mediator.Send(command);

            if(userId == Guid.Empty)
            {
                return BadRequest("A user with the same email already exists.");
            }

            return Ok(new { UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var token = await _mediator.Send(command);

            if(token == null)
            {
                return NotFound();
            }

            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var token = await _mediator.Send(command);

            if(token == null) 
            {
                return BadRequest();
            }
          
            return Ok(new {Token = token});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var user = await _mediator.Send(command);

            if (!user.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(new { message = "Password reset successful." });
        }

    }
}