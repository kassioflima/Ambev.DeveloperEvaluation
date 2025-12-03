using Ambev.DeveloperEvaluation.Integration.Common;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Carts;

/// <summary>
/// Integration tests for Carts API endpoints
/// Tests complete HTTP request/response cycles
/// </summary>
public class CartsApiIntegrationTests : IntegrationTestBase
{
    public CartsApiIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that POST /api/carts creates a cart successfully
    /// </summary>
    [Fact(DisplayName = "Given valid cart data When POST /api/carts Then returns 201 Created")]
    public async Task CreateCart_ValidData_Returns201Created()
    {
        // Arrange
        await CleanupAsync();
        
        // Create a user first
        var userRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var userContent = CreateJsonContent(userRequest);
        var userResponse = await Client.PostAsync("/api/users", userContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(userResponse);
        var userId = createdUser!.Data!.Id;
        
        // Create products first
        var productIds = new List<Guid>();
        for (int i = 0; i < 3; i++)
        {
            var productRequest = IntegrationTestData.GenerateValidCreateProductRequest();
            var productContent = CreateJsonContent(productRequest);
            var productResponse = await Client.PostAsync("/api/products", productContent);
            var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(productResponse);
            productIds.Add(createdProduct!.Data!.Id);
        }
        
        var request = IntegrationTestData.GenerateValidCreateCartRequest(userId, productIds);
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/carts", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<CreateCartResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().NotBeEmpty();
        responseBody.Data.UserId.Should().Be(request.UserId);
    }

    /// <summary>
    /// Tests that POST /api/carts with invalid data returns 400 Bad Request
    /// </summary>
    [Fact(DisplayName = "Given invalid cart data When POST /api/carts Then returns 400 Bad Request")]
    public async Task CreateCart_InvalidData_Returns400BadRequest()
    {
        // Arrange
        await CleanupAsync();
        var request = new CreateCartRequest(); // Empty request will fail validation
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/carts", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests that GET /api/carts/{id} retrieves a cart successfully
    /// </summary>
    [Fact(DisplayName = "Given existing cart When GET /api/carts/{id} Then returns 200 OK")]
    public async Task GetCart_ExistingCart_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        
        // Create a user first
        var userRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var userContent = CreateJsonContent(userRequest);
        var userResponse = await Client.PostAsync("/api/users", userContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(userResponse);
        var userId = createdUser!.Data!.Id;
        
        // Create products first
        var productIds = new List<Guid>();
        for (int i = 0; i < 3; i++)
        {
            var productRequest = IntegrationTestData.GenerateValidCreateProductRequest();
            var productContent = CreateJsonContent(productRequest);
            var productResponse = await Client.PostAsync("/api/products", productContent);
            var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(productResponse);
            productIds.Add(createdProduct!.Data!.Id);
        }
        
        var createRequest = IntegrationTestData.GenerateValidCreateCartRequest(userId, productIds);
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/carts", createContent);
        var createdCart = await ReadJsonResponse<ApiResponseWithData<CreateCartResponse>>(createResponse);
        var cartId = createdCart!.Data!.Id;

        // Act
        var response = await Client.GetAsync($"/api/carts/{cartId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<GetCartResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().Be(cartId);
        responseBody.Data.UserId.Should().Be(createRequest.UserId);
        responseBody.Data.Products.Should().HaveCount(createRequest.Products.Count);
    }

    /// <summary>
    /// Tests that GET /api/carts returns paginated list of carts
    /// </summary>
    [Fact(DisplayName = "Given carts exist When GET /api/carts Then returns paginated list")]
    public async Task GetCarts_WithCarts_ReturnsPaginatedList()
    {
        // Arrange
        await CleanupAsync();
        
        // Create a user first
        var userRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var userContent = CreateJsonContent(userRequest);
        var userResponse = await Client.PostAsync("/api/users", userContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(userResponse);
        var userId = createdUser!.Data!.Id;
        
        // Create products first
        var productIds = new List<Guid>();
        for (int i = 0; i < 3; i++)
        {
            var productRequest = IntegrationTestData.GenerateValidCreateProductRequest();
            var productContent = CreateJsonContent(productRequest);
            var productResponse = await Client.PostAsync("/api/products", productContent);
            var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(productResponse);
            productIds.Add(createdProduct!.Data!.Id);
        }
        
        // Create multiple carts
        for (int i = 0; i < 3; i++)
        {
            var request = IntegrationTestData.GenerateValidCreateCartRequest(userId, productIds);
            var content = CreateJsonContent(request);
            await Client.PostAsync("/api/carts", content);
        }

        // Act
        var response = await Client.GetAsync("/api/carts?_page=1&_size=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<GetCartsResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Data.Should().HaveCount(3);
        responseBody.Data.TotalItems.Should().Be(3);
    }

    /// <summary>
    /// Tests that PUT /api/carts/{id} updates a cart successfully
    /// </summary>
    [Fact(DisplayName = "Given existing cart When PUT /api/carts/{id} Then returns 200 OK")]
    public async Task UpdateCart_ExistingCart_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        
        // Create a user first
        var userRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var userContent = CreateJsonContent(userRequest);
        var userResponse = await Client.PostAsync("/api/users", userContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(userResponse);
        var userId = createdUser!.Data!.Id;
        
        // Create products first
        var productIds = new List<Guid>();
        for (int i = 0; i < 3; i++)
        {
            var productRequest = IntegrationTestData.GenerateValidCreateProductRequest();
            var productContent = CreateJsonContent(productRequest);
            var productResponse = await Client.PostAsync("/api/products", productContent);
            var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(productResponse);
            productIds.Add(createdProduct!.Data!.Id);
        }
        
        var createRequest = IntegrationTestData.GenerateValidCreateCartRequest(userId, productIds);
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/carts", createContent);
        var createdCart = await ReadJsonResponse<ApiResponseWithData<CreateCartResponse>>(createResponse);
        var cartId = createdCart!.Data!.Id;

        var updateRequest = IntegrationTestData.GenerateValidUpdateCartRequest(userId, productIds);
        var updateContent = CreateJsonContent(updateRequest);

        // Act
        var response = await Client.PutAsync($"/api/carts/{cartId}", updateContent);

        // Debug: capture error if any
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine($"Status: {response.StatusCode}");
            System.Console.WriteLine($"Error: {errorResponse}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<UpdateCartResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().Be(cartId);
        responseBody.Data.UserId.Should().Be(updateRequest.UserId);
    }

    /// <summary>
    /// Tests that DELETE /api/carts/{id} deletes a cart successfully
    /// </summary>
    [Fact(DisplayName = "Given existing cart When DELETE /api/carts/{id} Then returns 200 OK")]
    public async Task DeleteCart_ExistingCart_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        
        // Create a user first
        var userRequest = IntegrationTestData.GenerateValidCreateUserRequest();
        var userContent = CreateJsonContent(userRequest);
        var userResponse = await Client.PostAsync("/api/users", userContent);
        var createdUser = await ReadJsonResponse<ApiResponseWithData<CreateUserResponse>>(userResponse);
        var userId = createdUser!.Data!.Id;
        
        // Create products first
        var productIds = new List<Guid>();
        for (int i = 0; i < 3; i++)
        {
            var productRequest = IntegrationTestData.GenerateValidCreateProductRequest();
            var productContent = CreateJsonContent(productRequest);
            var productResponse = await Client.PostAsync("/api/products", productContent);
            var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(productResponse);
            productIds.Add(createdProduct!.Data!.Id);
        }
        
        var createRequest = IntegrationTestData.GenerateValidCreateCartRequest(userId, productIds);
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/carts", createContent);
        var createdCart = await ReadJsonResponse<ApiResponseWithData<CreateCartResponse>>(createResponse);
        var cartId = createdCart!.Data!.Id;

        // Act
        var response = await Client.DeleteAsync($"/api/carts/{cartId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponse>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();

        // Verify cart was deleted
        var getResponse = await Client.GetAsync($"/api/carts/{cartId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

