using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Utils
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            if (exception is ValidationException validationException)
            {
                status = HttpStatusCode.BadRequest;
                var errors = validationException.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });
                var errorResponse = new { Message = "Validation failed", Errors = errors };
                message = JsonSerializer.Serialize(errorResponse);
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = JsonSerializer.Serialize(new { Message = "An unexpected error occurred." });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(message);
        }
    }
}