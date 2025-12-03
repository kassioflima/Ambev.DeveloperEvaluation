using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Functional.Common;
using Ambev.DeveloperEvaluation.Functional.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Carts;

/// <summary>
/// Functional tests for cart creation flow
/// Tests the complete flow from command to database persistence
/// </summary>
public class CreateCartFunctionalTests : FunctionalTestBase
{
    private readonly IMediator _mediator;
    private readonly ICartRepository _cartRepository;

    public CreateCartFunctionalTests()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
        _cartRepository = ServiceProvider.GetRequiredService<ICartRepository>();
    }

    /// <summary>
    /// Tests that a complete cart creation flow works correctly
    /// </summary>
    [Fact(DisplayName = "Given valid cart data When creating cart Then cart is created successfully")]
    public async Task CreateCart_ValidData_ReturnsSuccess()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateCartCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        // Verify cart was persisted in database
        var persistedCart = await _cartRepository.GetByIdAsync(result.Id);
        persistedCart.Should().NotBeNull();
        persistedCart!.UserId.Should().Be(command.UserId);
        persistedCart.Date.Should().BeCloseTo(command.Date, TimeSpan.FromSeconds(1));
        persistedCart.Products.Should().HaveCount(command.Products.Count);
    }

    /// <summary>
    /// Tests that cart creation fails with invalid data
    /// </summary>
    [Fact(DisplayName = "Given invalid cart data When creating cart Then throws validation exception")]
    public async Task CreateCart_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateCartCommand(); // Empty command will fail validation
        await CleanupAsync();

        // Act
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that cart items are persisted correctly
    /// </summary>
    [Fact(DisplayName = "Given cart with products When creating cart Then products are persisted correctly")]
    public async Task CreateCart_WithProducts_PersistsProducts()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateCartCommand(productCount: 3);
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedCart = await _cartRepository.GetByIdAsync(result.Id);
        persistedCart.Should().NotBeNull();
        persistedCart!.Products.Should().HaveCount(3);
        
        foreach (var product in command.Products)
        {
            persistedCart.Products.Should().Contain(p => 
                p.ProductId == product.ProductId && 
                p.Quantity == product.Quantity);
        }
    }

    /// <summary>
    /// Tests that cart CreatedAt is set automatically
    /// </summary>
    [Fact(DisplayName = "Given cart creation When creating cart Then CreatedAt is set automatically")]
    public async Task CreateCart_ValidData_CreatedAtIsSet()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateCartCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedCart = await _cartRepository.GetByIdAsync(result.Id);
        persistedCart.Should().NotBeNull();
        persistedCart!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Tests that cart items reference the correct cart ID
    /// </summary>
    [Fact(DisplayName = "Given cart creation When creating cart Then cart items reference cart ID")]
    public async Task CreateCart_WithProducts_ItemsReferenceCartId()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateCartCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedCart = await _cartRepository.GetByIdAsync(result.Id);
        persistedCart.Should().NotBeNull();
        persistedCart!.Products.Should().OnlyContain(item => item.CartId == result.Id);
    }
}

