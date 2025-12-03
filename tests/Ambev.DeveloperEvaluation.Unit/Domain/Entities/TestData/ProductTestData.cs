using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Product entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Product entities.
    /// The generated products will have valid:
    /// - Title (using commerce product names)
    /// - Price (positive decimal values)
    /// - Description (using lorem text)
    /// - Category (using commerce categories)
    /// - Image (using image URLs)
    /// - Rating (valid rating with rate between 0-5 and count >= 0)
    /// </summary>
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => f.Random.Guid())
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(0.01m, 10000.00m))
        .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.Rating, f => new ProductRating(
            f.Random.Decimal(0, 5),
            f.Random.Int(0, 1000)))
        .RuleFor(p => p.CreatedAt, f => f.Date.Past())
        .RuleFor(p => p.UpdatedAt, f => f.Date.Recent().OrNull(f, 0.3f));

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        return ProductFaker.Generate();
    }

    /// <summary>
    /// Generates a valid product title using Faker.
    /// </summary>
    /// <returns>A valid product title.</returns>
    public static string GenerateValidTitle()
    {
        return new Faker().Commerce.ProductName();
    }

    /// <summary>
    /// Generates a valid product price.
    /// </summary>
    /// <returns>A valid product price (positive decimal).</returns>
    public static decimal GenerateValidPrice()
    {
        return new Faker().Random.Decimal(0.01m, 10000.00m);
    }

    /// <summary>
    /// Generates a valid product category.
    /// </summary>
    /// <returns>A valid product category.</returns>
    public static string GenerateValidCategory()
    {
        return new Faker().Commerce.Categories(1)[0];
    }

    /// <summary>
    /// Generates a valid ProductRating.
    /// </summary>
    /// <returns>A valid ProductRating with rate between 0-5 and count >= 0.</returns>
    public static ProductRating GenerateValidRating()
    {
        var faker = new Faker();
        return new ProductRating(
            faker.Random.Decimal(0, 5),
            faker.Random.Int(0, 1000));
    }

    /// <summary>
    /// Generates an invalid product price (negative or zero).
    /// </summary>
    /// <returns>An invalid product price.</returns>
    public static decimal GenerateInvalidPrice()
    {
        return new Faker().Random.Decimal(-1000, 0);
    }

    /// <summary>
    /// Generates an invalid ProductRating with rate outside 0-5 range.
    /// </summary>
    /// <returns>An invalid ProductRating.</returns>
    public static ProductRating GenerateInvalidRating()
    {
        var faker = new Faker();
        return new ProductRating(
            faker.Random.Decimal(5.01m, 10m),
            faker.Random.Int(-100, -1));
    }
}

