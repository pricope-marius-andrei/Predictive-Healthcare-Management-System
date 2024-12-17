using Application.Utils;
using Predictive_Healthcare_Management_System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://clinquant-kataifi-aee780.netlify.app") // Allow Angular app origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);



var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAngularApp");

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<Application.Middleware.TokenValidationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();



public partial class Program {}