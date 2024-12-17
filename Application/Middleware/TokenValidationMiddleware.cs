using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var validationResult = ValidateToken(token);

                if (!validationResult.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    // Serialize the error response manually
                    var errorResponse = new
                    {
                        Success = false,
                        Message = validationResult.ErrorMessage
                    };

                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }

                // Attach user information to the context (optional)
                context.Items["User"] = validationResult.ClaimsPrincipal;
            }

            await _next(context);
        }

        private TokenValidationResult ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Prevent clock skew issues
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);

                // Ensure the token is a JWT token
                if (validatedToken is JwtSecurityToken jwtToken &&
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return TokenValidationResult.Failure("Invalid token algorithm.");
                }

                return TokenValidationResult.Success(principal);
            }
            catch (SecurityTokenExpiredException)
            {
                return TokenValidationResult.Failure("Token has expired.");
            }
            catch (Exception)
            {
                return TokenValidationResult.Failure("Invalid token.");
            }
        }

        private class TokenValidationResult
        {
            public bool IsValid { get; }
            public string ErrorMessage { get; }
            public ClaimsPrincipal ClaimsPrincipal { get; }

            private TokenValidationResult(bool isValid, ClaimsPrincipal claimsPrincipal, string errorMessage)
            {
                IsValid = isValid;
                ClaimsPrincipal = claimsPrincipal;
                ErrorMessage = errorMessage;
            }

            public static TokenValidationResult Success(ClaimsPrincipal claimsPrincipal) =>
                new TokenValidationResult(true, claimsPrincipal, null);

            public static TokenValidationResult Failure(string errorMessage) =>
                new TokenValidationResult(false, null, errorMessage);
        }
    }
}
