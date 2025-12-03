using Ambev.DeveloperEvaluation.Integration.Common;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Products;

/// <summary>
/// Integration tests for Products API endpoints
/// Tests complete HTTP request/response cycles
/// </summary>
public class ProductsApiIntegrationTests : IntegrationTestBase
{
    public ProductsApiIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that POST /api/products creates a product successfully
    /// </summary>
    [Fact(DisplayName = "Given valid product data When POST /api/products Then returns 201 Created")]
    public async Task CreateProduct_ValidData_Returns201Created()
    {
        // Arrange
        await CleanupAsync();
        var request = IntegrationTestData.GenerateValidCreateProductRequest();
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().NotBeEmpty();
        responseBody.Data.Title.Should().Be(request.Title);
    }

    /// <summary>
    /// Tests that POST /api/products with invalid data returns 400 Bad Request
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When POST /api/products Then returns 400 Bad Request")]
    public async Task CreateProduct_InvalidData_Returns400BadRequest()
    {
        // Arrange
        await CleanupAsync();
        var request = new CreateProductRequest(); // Empty request will fail validation
        var content = CreateJsonContent(request);

        // Act
        var response = await Client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests that GET /api/products/{id} retrieves a product successfully
    /// </summary>
    [Fact(DisplayName = "Given existing product When GET /api/products/{id} Then returns 200 OK")]
    public async Task GetProduct_ExistingProduct_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateProductRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/products", createContent);
        var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(createResponse);
        var productId = createdProduct!.Data!.Id;

        // Act
        var response = await Client.GetAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<GetProductResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().Be(productId);
        responseBody.Data.Title.Should().Be(createRequest.Title);
    }

    /// <summary>
    /// Tests that GET /api/products returns paginated list of products
    /// </summary>
    [Fact(DisplayName = "Given products exist When GET /api/products Then returns paginated list")]
    public async Task GetProducts_WithProducts_ReturnsPaginatedList()
    {
        // Arrange
        await CleanupAsync();
        
        // Create multiple products
        for (int i = 0; i < 3; i++)
        {
            var request = IntegrationTestData.GenerateValidCreateProductRequest();
            var content = CreateJsonContent(request);
            await Client.PostAsync("/api/products", content);
        }

        // Act
        var response = await Client.GetAsync("/api/products?_page=1&_size=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<GetProductsResponse>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Data.Should().HaveCount(3);
        responseBody.TotalItems.Should().Be(3);
    }

    /// <summary>
    /// Tests that GET /api/products/categories returns list of categories
    /// </summary>
    [Fact(DisplayName = "Given products exist When GET /api/products/categories Then returns categories list")]
    public async Task GetCategories_WithProducts_ReturnsCategoriesList()
    {
        // Arrange
        await CleanupAsync();
        
        // Create products with different categories
        var request1 = IntegrationTestData.GenerateValidCreateProductRequest();
        request1.Category = "Electronics";
        await Client.PostAsync("/api/products", CreateJsonContent(request1));

        var request2 = IntegrationTestData.GenerateValidCreateProductRequest();
        request2.Category = "Clothing";
        await Client.PostAsync("/api/products", CreateJsonContent(request2));

        // Act
        var response = await Client.GetAsync("/api/products/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var categories = await ReadJsonResponse<List<string>>(response);
        categories.Should().NotBeNull();
        categories!.Should().Contain("Electronics");
        categories.Should().Contain("Clothing");
    }

    /// <summary>
    /// Tests that PUT /api/products/{id} updates a product successfully
    /// </summary>
    [Fact(DisplayName = "Given existing product When PUT /api/products/{id} Then returns 200 OK")]
    public async Task UpdateProduct_ExistingProduct_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateProductRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/products", createContent);
        var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(createResponse);
        var productId = createdProduct!.Data!.Id;

        var updateRequest = IntegrationTestData.GenerateValidUpdateProductRequest();
        var updateContent = CreateJsonContent(updateRequest);

        // Act
        var response = await Client.PutAsync($"/api/products/{productId}", updateContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponseWithData<UpdateProductResponse>>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();
        responseBody.Data.Should().NotBeNull();
        responseBody.Data!.Id.Should().Be(productId);
        responseBody.Data.Title.Should().Be(updateRequest.Title);
    }

    /// <summary>
    /// Tests that DELETE /api/products/{id} deletes a product successfully
    /// </summary>
    [Fact(DisplayName = "Given existing product When DELETE /api/products/{id} Then returns 200 OK")]
    public async Task DeleteProduct_ExistingProduct_Returns200OK()
    {
        // Arrange
        await CleanupAsync();
        var createRequest = IntegrationTestData.GenerateValidCreateProductRequest();
        var createContent = CreateJsonContent(createRequest);
        var createResponse = await Client.PostAsync("/api/products", createContent);
        var createdProduct = await ReadJsonResponse<ApiResponseWithData<CreateProductResponse>>(createResponse);
        var productId = createdProduct!.Data!.Id;

        // Act
        var response = await Client.DeleteAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await ReadJsonResponse<ApiResponse>(response);
        responseBody.Should().NotBeNull();
        responseBody!.Success.Should().BeTrue();

        // Verify product was deleted
        var getResponse = await Client.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

