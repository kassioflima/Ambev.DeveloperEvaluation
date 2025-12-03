using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover entity creation, property validation, and business rules.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that a product is created with valid properties.
    /// </summary>
    [Fact(DisplayName = "Product should be created with valid properties")]
    public void Given_ValidProductData_When_Created_Then_ShouldHaveValidProperties()
    {
        // Arrange & Act
        var product = ProductTestData.GenerateValidProduct();

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Title.Should().NotBeNullOrEmpty();
        product.Price.Should().BeGreaterThan(0);
        product.Description.Should().NotBeNullOrEmpty();
        product.Category.Should().NotBeNullOrEmpty();
        product.CreatedAt.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
    }

    /// <summary>
    /// Tests that CreatedAt is set automatically when a product is created.
    /// </summary>
    [Fact(DisplayName = "Product CreatedAt should be set automatically on creation")]
    public void Given_NewProduct_When_Created_Then_CreatedAtShouldBeSet()
    {
        // Arrange & Act
        var product = new Product
        {
            Title = ProductTestData.GenerateValidTitle(),
            Price = ProductTestData.GenerateValidPrice(),
            Description = "Test description",
            Category = ProductTestData.GenerateValidCategory()
        };

        // Assert
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that a product can have a rating.
    /// </summary>
    [Fact(DisplayName = "Product should support rating")]
    public void Given_Product_When_RatingIsSet_Then_ShouldHaveRating()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var rating = ProductTestData.GenerateValidRating();

        // Act
        product.Rating = rating;

        // Assert
        product.Rating.Should().NotBeNull();
        product.Rating.Rate.Should().BeInRange(0, 5);
        product.Rating.Count.Should().BeGreaterThanOrEqualTo(0);
    }

    /// <summary>
    /// Tests that a product can have null rating.
    /// </summary>
    [Fact(DisplayName = "Product should allow null rating")]
    public void Given_Product_When_RatingIsNull_Then_ShouldBeValid()
    {
        // Arrange & Act
        var product = new Product
        {
            Title = ProductTestData.GenerateValidTitle(),
            Price = ProductTestData.GenerateValidPrice(),
            Description = "Test description",
            Category = ProductTestData.GenerateValidCategory(),
            Rating = null
        };

        // Assert
        product.Rating.Should().BeNull();
    }

    /// <summary>
    /// Tests that UpdatedAt can be set when product is updated.
    /// </summary>
    [Fact(DisplayName = "Product UpdatedAt should be set when updated")]
    public void Given_Product_When_Updated_Then_UpdatedAtShouldBeSet()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var originalUpdatedAt = product.UpdatedAt;

        // Act
        product.UpdatedAt = DateTime.UtcNow;

        // Assert
        product.UpdatedAt.Should().NotBeNull();
        product.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    /// <summary>
    /// Tests that product properties can be modified.
    /// </summary>
    [Fact(DisplayName = "Product properties should be modifiable")]
    public void Given_Product_When_PropertiesAreModified_Then_ShouldReflectChanges()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var newTitle = ProductTestData.GenerateValidTitle();
        var newPrice = ProductTestData.GenerateValidPrice();
        var newCategory = ProductTestData.GenerateValidCategory();

        // Act
        product.Title = newTitle;
        product.Price = newPrice;
        product.Category = newCategory;

        // Assert
        product.Title.Should().Be(newTitle);
        product.Price.Should().Be(newPrice);
        product.Category.Should().Be(newCategory);
    }

    /// <summary>
    /// Tests that product inherits from BaseEntity.
    /// </summary>
    [Fact(DisplayName = "Product should inherit from BaseEntity")]
    public void Given_Product_When_Created_Then_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var product = new Product
        {
            Id = Guid.NewGuid()
        };

        // Assert
        product.Should().BeAssignableTo<BaseEntity>();
        product.Id.Should().NotBeEmpty();
    }
}

