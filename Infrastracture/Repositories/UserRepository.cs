using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.UseCases.Authentication;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Login(User user)
        {
            var doctor = await _context.Set<Doctor>().SingleOrDefaultAsync(u => u.Email == user.Email);
            var patient = await _context.Set<Patient>().SingleOrDefaultAsync(u => u.Email == user.Email);

            if (doctor == null && patient == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
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
                throw new UnauthorizedAccessException("Invalid credentials");
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
            return tokenHandler.WriteToken(token);
        }

        public async Task<Guid> Register(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}