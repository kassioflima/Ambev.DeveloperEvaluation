using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Cart entity class.
/// Tests cover entity creation, property validation, and business rules.
/// </summary>
public class CartTests
{
    /// <summary>
    /// Tests that a cart is created with valid properties.
    /// </summary>
    [Fact(DisplayName = "Cart should be created with valid properties")]
    public void Given_ValidCartData_When_Created_Then_ShouldHaveValidProperties()
    {
        // Arrange & Act
        var cart = CartTestData.GenerateValidCart();

        // Assert
        cart.Should().NotBeNull();
        cart.Id.Should().NotBeEmpty();
        cart.UserId.Should().NotBeEmpty();
        cart.Date.Should().BeBefore(DateTime.UtcNow.AddDays(1));
        cart.Products.Should().NotBeNull();
        cart.CreatedAt.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
    }

    /// <summary>
    /// Tests that CreatedAt and Date are set automatically when a cart is created.
    /// </summary>
    [Fact(DisplayName = "Cart CreatedAt and Date should be set automatically on creation")]
    public void Given_NewCart_When_Created_Then_CreatedAtAndDateShouldBeSet()
    {
        // Arrange & Act
        var cart = new Cart
        {
            UserId = CartTestData.GenerateValidUserId()
        };

        // Assert
        cart.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        cart.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that a cart can have multiple products.
    /// </summary>
    [Fact(DisplayName = "Cart should support multiple products")]
    public void Given_Cart_When_ProductsAreAdded_Then_ShouldContainProducts()
    {
        // Arrange
        var cart = new Cart
        {
            UserId = CartTestData.GenerateValidUserId()
        };
        var product1 = CartTestData.GenerateValidCartItem(cart.Id);
        var product2 = CartTestData.GenerateValidCartItem(cart.Id);

        // Act
        cart.Products.Add(product1);
        cart.Products.Add(product2);

        // Assert
        cart.Products.Should().HaveCount(2);
        cart.Products.Should().Contain(product1);
        cart.Products.Should().Contain(product2);
    }

    /// <summary>
    /// Tests that cart items reference the correct cart ID.
    /// </summary>
    [Fact(DisplayName = "Cart items should reference the correct cart ID")]
    public void Given_Cart_When_ItemsAreAdded_Then_ItemsShouldReferenceCartId()
    {
        // Arrange
        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = CartTestData.GenerateValidUserId()
        };
        var item = CartTestData.GenerateValidCartItem(cart.Id);

        // Act
        cart.Products.Add(item);

        // Assert
        cart.Products.Should().Contain(item);
        item.CartId.Should().Be(cart.Id);
    }

    /// <summary>
    /// Tests that a cart can be empty (no products).
    /// </summary>
    [Fact(DisplayName = "Cart should allow empty product list")]
    public void Given_Cart_When_NoProducts_Then_ShouldBeValid()
    {
        // Arrange & Act
        var cart = new Cart
        {
            UserId = CartTestData.GenerateValidUserId()
        };

        // Assert
        cart.Products.Should().NotBeNull();
        cart.Products.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that UpdatedAt can be set when cart is updated.
    /// </summary>
    [Fact(DisplayName = "Cart UpdatedAt should be set when updated")]
    public void Given_Cart_When_Updated_Then_UpdatedAtShouldBeSet()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var originalUpdatedAt = cart.UpdatedAt;

        // Act
        cart.UpdatedAt = DateTime.UtcNow;

        // Assert
        cart.UpdatedAt.Should().NotBeNull();
        cart.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that cart properties can be modified.
    /// </summary>
    [Fact(DisplayName = "Cart properties should be modifiable")]
    public void Given_Cart_When_PropertiesAreModified_Then_ShouldReflectChanges()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();
        var newUserId = CartTestData.GenerateValidUserId();
        var newDate = DateTime.UtcNow.AddDays(-1);

        // Act
        cart.UserId = newUserId;
        cart.Date = newDate;

        // Assert
        cart.UserId.Should().Be(newUserId);
        cart.Date.Should().Be(newDate);
    }

    /// <summary>
    /// Tests that cart inherits from BaseEntity.
    /// </summary>
    [Fact(DisplayName = "Cart should inherit from BaseEntity")]
    public void Given_Cart_When_Created_Then_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var cart = new Cart
        {
            Id = Guid.NewGuid()
        };

        // Assert
        cart.Should().BeAssignableTo<BaseEntity>();
        cart.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that CartItem is created with a valid ID.
    /// </summary>
    [Fact(DisplayName = "CartItem should be created with valid ID")]
    public void Given_CartItem_When_Created_Then_ShouldHaveValidId()
    {
        // Arrange & Act
        var cartItem = new CartItem();

        // Assert
        cartItem.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that CartItem can be associated with a cart.
    /// </summary>
    [Fact(DisplayName = "CartItem should be associated with a cart")]
    public void Given_CartItem_When_AssociatedWithCart_Then_ShouldReferenceCart()
    {
        // Arrange
        var cart = new Cart { Id = Guid.NewGuid() };
        var cartItem = CartTestData.GenerateValidCartItem(cart.Id);

        // Act
        cartItem.Cart = cart;

        // Assert
        cartItem.Cart.Should().Be(cart);
        cartItem.CartId.Should().Be(cart.Id);
    }
}

