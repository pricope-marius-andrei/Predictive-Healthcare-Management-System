using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock or configure additional services if needed
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Register_ReturnsSuccess()
    {
        // Arrange
        var newUser = new
        {
            UserType = "Standard",
            Email = "test@example.com",
            Password = "Password123!",
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890"
        };
        var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/auth/register", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("UserId", responseString);
    }

    [Fact]
    public async Task Login_ReturnsToken()
    {
        // Arrange
        var loginUser = new
        {
            Email = "doctor123@gmail.com",
            Password = "doctor123!"
        };
        var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/auth/login", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Token", responseString);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsToken()
    {
        // Arrange
        var forgotPassword = new
        {
            Email = "test@example.com",
            ClientUrl = "http://localhost:4200"
        };
        var content = new StringContent(JsonConvert.SerializeObject(forgotPassword), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/auth/forgot-password", content);

        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, $"Expected success status code but got {response.StatusCode}. Response: {responseString}");
        Assert.Contains("Token", responseString);
    }

    [Fact]
    public async Task ResetPassword_ReturnsSuccess()
    {
        // Arrange
        var resetPassword = new
        {
            Email = "test@example.com",
            Password = "NewPassword123!",
            ConfirmPassword = "NewPassword123!",
            Token = "some-valid-token"
        };
        var content = new StringContent(JsonConvert.SerializeObject(resetPassword), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/auth/reset-password", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Password reset successful", responseString);
    }
}

