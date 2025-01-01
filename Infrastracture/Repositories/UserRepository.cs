using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Common;
using Application.UseCases.Authentication;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string InvalidCredentials = "Invalid credentials.";
        private const string UserExists = "The user with same email exists";
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<Result<string>> Login(User user)
        {
            try
            {
                var doctor = await _context.Set<Doctor>().SingleOrDefaultAsync(u => u.Email == user.Email);
                var patient = await _context.Set<Patient>().SingleOrDefaultAsync(u => u.Email == user.Email);

                if (doctor == null && patient == null)
                {
                    return Result<string>.Failure(InvalidCredentials);
                }

                EUserType userType;
                User existingUser;

                if (doctor != null)
                {
                    existingUser = doctor;
                    userType = EUserType.Doctor;
                }
                else
                {
                    existingUser = patient;
                    userType = EUserType.Patient;
                }

                if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                {
                    return Result<string>.Failure(InvalidCredentials);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, existingUser.Id.ToString()),
                    new Claim("UserType", userType.ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Result<string>.Success(tokenHandler.WriteToken(token));
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    throw new Exception(UserExists);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(user.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        public async Task<Result<User>> ResetPassword(string email, string newPassword, string token)
        {
            if(token == null)
            {
                return Result<User>.Failure("The token is invalid");
            }

           var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Result<User>.Failure("The user doesn't exist");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return Result<User>.Success(user);
        }

        public async Task<Result<string>> SendPasswordResetEmailAsync(string email, string resetLink)
        {
            string subject = "Password Reset Request";
            string body = $"<p>Click the link below to reset your password:</p>" +
                          $"<a href='{resetLink}'>Reset Password</a>";


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if(user == null)
            {
                return Result<string>.Failure("The user doesn't exist");
            }

            var response =  await _emailService.SendEmailAsync(email, subject, body);

            if(response.IsSuccess)
            {
                return Result<string>.Success("Password reset link sent to your email.");
            }
            else
            {
                return Result<string>.Failure(null);
            }
        }

    }
}