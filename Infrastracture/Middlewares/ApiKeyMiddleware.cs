using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Check for subscription key in header or query parameter
            var subscriptionKey = httpContext.Request.Headers["Ocp-Apim-Subscription-Key"]
                                  .FirstOrDefault()
                                  ?? httpContext.Request.Query["subscription-key"];

            if (string.IsNullOrEmpty(subscriptionKey) || subscriptionKey != "your-valid-subscription-key")
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized: Invalid subscription key");
                return;
            }

            await _next(httpContext); // Proceed if valid key
        }
    }
}