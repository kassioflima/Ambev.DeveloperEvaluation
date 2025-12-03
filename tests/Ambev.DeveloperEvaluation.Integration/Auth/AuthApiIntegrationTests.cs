using Ambev.DeveloperEvaluation.Integration.Common;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Auth;

/// <summary>
/// Integration tests for Auth API endpoints
/// Tests complete HTTP request/response cycles
/// </summary>
public class AuthApiIntegrationTests : IntegrationTestBase
{
    public AuthApiIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that POST /api/auth authenticates a user successfully
    /// </summary>
    [Fact(DisplayName = "Given valid credentials When POST /api/auth Then returns 200 OK with token")]
    public async Task AuthenticateUser_ValidCredentials_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        
        // Create user first
        var createRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        createRequest.Status = "Active"; // User must be active to authenticate
        var createContent = CreateJsonContent(createRequest);
        await Client.PostAsync("/api/users", createContent);

        var authRequest = IntegrationTestData.GenerateValidAuthenticateUserRequest(
            createRequest.Email, 
            createRequest.Password);
        var authContent = CreateJsonContent(authRequest);

        // Act
        var response = await Client.PostAsync("/api/auth", authContent);
        
        // Debug: Read response body to see what error occurred
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            var errorResponse = await ReadJsonResponse<ApiResponse>(response);
            throw new Exception($"Expected 200 but got {(int)response.StatusCode}. Error: {errorResponse?.Message ?? errorBody}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<ApiResponseWithData<AuthenticateUserResponse>>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Data.Should().NotBeNull();
        responseBody.Data.Data!.Token.Should().NotBeNullOrEmpty();
        responseBody.Data.Data.Email.Should().Be(createRequest.Email);
    }

    /// <summary>
    /// Tests that POST /api/auth with invalid email returns 401 Unauthorized
    /// </summary>
    [Fact(DisplayName = "Given invalid email When POST /api/auth Then returns 401 Unauthorized")]
    public async Task AuthenticateUser_InvalidEmail_Returns401Unauthorized()
    {
        // Arrange
        await CleanupAsync();
        var authRequest = IntegrationTestData.GenerateValidAuthenticateUserRequest(
            "nonexistent@example.com",
            "Test@123");
        var authContent = CreateJsonContent(authRequest);

        // Act
        var response = await Client.PostAsync("/api/auth", authContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that POST /api/auth with invalid password returns 401 Unauthorized
    /// </summary>
    [Fact(DisplayName = "Given invalid password When POST /api/auth Then returns 401 Unauthorized")]
    public async Task AuthenticateUser_InvalidPassword_Returns401Unauthorized()
    {
        // Arrange
        await CleanupAsync();
        
        // Create user first
        var createRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        createRequest.Status = "Active";
        var createContent = CreateJsonContent(createRequest);
        await Client.PostAsync("/api/users", createContent);

        var authRequest = IntegrationTestData.GenerateValidAuthenticateUserRequest(
            createRequest.Email,
            "WrongPassword@123");
        var authContent = CreateJsonContent(authRequest);

        // Act
        var response = await Client.PostAsync("/api/auth", authContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that POST /api/auth with invalid data returns 400 Bad Request
    /// </summary>
    [Fact(DisplayName = "Given invalid authentication data When POST /api/auth Then returns 400 Bad Request")]
    public async Task AuthenticateUser_InvalidData_Returns400BadRequest()
    {
        // Arrange
        await CleanupAsync();
        var authRequest = new AuthenticateUserRequest(); // Empty request
        var authContent = CreateJsonContent(authRequest);

        // Act
        var response = await Client.PostAsync("/api/auth", authContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

