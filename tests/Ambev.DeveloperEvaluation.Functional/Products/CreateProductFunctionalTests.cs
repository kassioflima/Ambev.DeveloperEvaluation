using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Functional.Common;
using Ambev.DeveloperEvaluation.Functional.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Products;

/// <summary>
/// Functional tests for product creation flow
/// Tests the complete flow from command to database persistence
/// </summary>
public class CreateProductFunctionalTests : FunctionalTestBase
{
    private readonly IMediator _mediator;
    private readonly IProductRepository _productRepository;

    public CreateProductFunctionalTests()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
        _productRepository = ServiceProvider.GetRequiredService<IProductRepository>();
    }

    /// <summary>
    /// Tests that a complete product creation flow works correctly
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then product is created successfully")]
    public async Task CreateProduct_ValidData_ReturnsSuccess()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateProductCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        // Verify product was persisted in database
        var persistedProduct = await _productRepository.GetByIdAsync(result.Id);
        persistedProduct.Should().NotBeNull();
        persistedProduct!.Title.Should().Be(command.Title);
        persistedProduct.Price.Should().Be(command.Price);
        persistedProduct.Description.Should().Be(command.Description);
        persistedProduct.Category.Should().Be(command.Category);
        persistedProduct.Image.Should().Be(command.Image);
    }

    /// <summary>
    /// Tests that product creation fails with invalid data
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task CreateProduct_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateProductCommand(); // Empty command will fail validation
        await CleanupAsync();

        // Act
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that product rating is persisted correctly
    /// </summary>
    [Fact(DisplayName = "Given product with rating When creating product Then rating is persisted")]
    public async Task CreateProduct_WithRating_PersistsRating()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateProductCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedProduct = await _productRepository.GetByIdAsync(result.Id);
        persistedProduct.Should().NotBeNull();
        persistedProduct!.Rating.Should().NotBeNull();
        persistedProduct.Rating!.Rate.Should().Be(command.Rating!.Rate);
        persistedProduct.Rating.Count.Should().Be(command.Rating.Count);
    }

    /// <summary>
    /// Tests that product CreatedAt is set automatically
    /// </summary>
    [Fact(DisplayName = "Given product creation When creating product Then CreatedAt is set automatically")]
    public async Task CreateProduct_ValidData_CreatedAtIsSet()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateProductCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedProduct = await _productRepository.GetByIdAsync(result.Id);
        persistedProduct.Should().NotBeNull();
        persistedProduct!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Tests that multiple products can be created
    /// </summary>
    [Fact(DisplayName = "Given multiple products When creating products Then all products are persisted")]
    public async Task CreateProduct_MultipleProducts_AllPersisted()
    {
        // Arrange
        var command1 = FunctionalTestData.GenerateValidCreateProductCommand();
        var command2 = FunctionalTestData.GenerateValidCreateProductCommand();
        await CleanupAsync();

        // Act
        var result1 = await _mediator.Send(command1, CancellationToken.None);
        var result2 = await _mediator.Send(command2, CancellationToken.None);

        // Assert
        var persistedProduct1 = await _productRepository.GetByIdAsync(result1.Id);
        var persistedProduct2 = await _productRepository.GetByIdAsync(result2.Id);
        
        persistedProduct1.Should().NotBeNull();
        persistedProduct2.Should().NotBeNull();
        persistedProduct1!.Title.Should().Be(command1.Title);
        persistedProduct2!.Title.Should().Be(command2.Title);
    }
}

