using Ambev.DeveloperEvaluation.Integration.Common;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Users;

/// <summary>
/// Integration tests for Users API endpoints
/// Tests complete HTTP request/response cycles
/// </summary>
public class UsersApiIntegrationTests : IntegrationTestBase
{
    public UsersApiIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that POST /api/users creates a user successfully
    /// </summary>
    [Fact(DisplayName = "Given valid user data When POST /api/users Then returns 201 Created")]
    public async Task CreateUser_ValidData_Returns201Created()
    {
        // Arrange
        await CleanupAsync();
        var request = IntegrationTestData.GenerateValidCreateUserRequest();
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/users", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().NotBeEmpty();
        responseBody.Data.Email.Should().Be(request.Email);
    }

    /// <summary>
    /// Tests that POST /api/users with invalid data returns 400 Bad Request
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When POST /api/users Then returns 400 Bad Request")]
    public async Task CreateUser_InvalidData_Returns400BadRequest()
    {
        // Arrange
        await CleanupAsync();
        var request = new CreateUserRequest(); // Empty request will fail validation
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/users", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests that GET /api/users/{id} retrieves a user successfully
    /// </summary>
    [Fact(DisplayName = "Given existing user When GET /api/users/{id} Then returns 200 OK")]
    public async Task GetUser_ExistingUser_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/users", createContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(createResponse);
        var userId = createdUser!.Data!.Id;

        // Act
        var response = await Client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<GetUserResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().Be(userId);
        responseBody.Data.Email.Should().Be(createRequest.Email);
    }

    /// <summary>
    /// Tests that GET /api/users/{id} with non-existent user returns 404 Not Found
    /// </summary>
    [Fact(DisplayName = "Given non-existent user When GET /api/users/{id} Then returns 404 Not Found")]
    public async Task GetUser_NonExistentUser_Returns404NotFound()
    {
        // Arrange
        await CleanupAsync();
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/users/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests that GET /api/users returns paginated list of users
    /// </summary>
    [Fact(DisplayName = "Given users exist When GET /api/users Then returns paginated list")]
    public async Task GetUsers_WithUsers_ReturnsPaginatedList()
    {
        // Arrange
        await CleanupAsync();
        
        // Create multiple users
        for (int i = 0; i < 3; i++)
        {
            var request = IntegrationTestData.GenerateValidCreateUserRequest();
            var content = CreateJsonContent(request);
            await Client.PostAsync("/api/users", content);
        }

        // Act
        var response = await Client.GetAsync("/api/users?_page=1&_size=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<GetUsersResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Data.Should().HaveCount(3);
        responseBody.Data.TotalItems.Should().Be(3);
    }

    /// <summary>
    /// Tests that PUT /api/users/{id} updates a user successfully
    /// </summary>
    [Fact(DisplayName = "Given existing user When PUT /api/users/{id} Then returns 200 OK")]
    public async Task UpdateUser_ExistingUser_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/users", createContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(createResponse);
        var userId = createdUser!.Data!.Id;

        var updateRequest = IntegrationTestData.GenerateValidUpdateUserRequest();
        var updateContent = CreateJsonContent(updateRequest);

        // Act
        var response = await Client.PutAsync($"/api/users/{userId}", updateContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<UpdateUserResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data.Email.Should().Be(updateRequest.Email);
    }

    /// <summary>
    /// Tests that DELETE /api/users/{id} deletes a user successfully
    /// </summary>
    [Fact(DisplayName = "Given existing user When DELETE /api/users/{id} Then returns 200 OK")]
    public async Task DeleteUser_ExistingUser_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/users", createContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(createResponse);
        var userId = createdUser!.Data!.Id;

        // Act
        var response = await Client.DeleteAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponse>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();

        // Verify user was deleted
        var getResponse = await Client.GetAsync($"/api/users/{userId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

