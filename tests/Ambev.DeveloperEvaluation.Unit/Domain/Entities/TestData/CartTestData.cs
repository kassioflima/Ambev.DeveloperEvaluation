using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Cart entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CartTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Cart entities.
    /// The generated carts will have valid:
    /// - UserId (positive integer)
    /// - Date (recent date)
    /// - Products (list of cart items with valid product IDs and quantities)
    /// </summary>
    private static readonly Faker<Cart> CartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Date, f => f.Date.Recent())
        .RuleFor(c => c.Products, f => GenerateCartItems(f, f.Random.Guid(), f.Random.Int(1, 5)))
        .RuleFor(c => c.CreatedAt, f => f.Date.Past())
        .RuleFor(c => c.UpdatedAt, f => f.Date.Recent().OrNull(f, 0.3f));

    /// <summary>
    /// Generates a list of valid CartItem entities.
    /// </summary>
    /// <param name="faker">The Faker instance to use.</param>
    /// <param name="cartId">The cart ID to associate items with.</param>
    /// <param name="itemCount">The number of items to generate.</param>
    /// <returns>A list of valid CartItem entities.</returns>
    private static List<CartItem> GenerateCartItems(Faker faker, Guid cartId, int itemCount)
    {
        var items = new List<CartItem>();
        for (int i = 0; i < itemCount; i++)
        {
            items.Add(new CartItem
            {
                Id = faker.Random.Guid(),
                CartId = cartId,
                ProductId = faker.Random.Guid(),
                Quantity = faker.Random.Int(1, 10)
            });
        }
        return items;
    }

    /// <summary>
    /// Generates a valid Cart entity with randomized data.
    /// The generated cart will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid Cart entity with randomly generated data.</returns>
    public static Cart GenerateValidCart()
    {
        var cart = CartFaker.Generate();
        // Ensure all cart items reference the cart ID
        foreach (var item in cart.Products)
        {
            item.CartId = cart.Id;
        }
        return cart;
    }

    /// <summary>
    /// Generates a valid CartItem entity.
    /// </summary>
    /// <param name="cartId">The cart ID to associate the item with.</param>
    /// <returns>A valid CartItem entity.</returns>
    public static CartItem GenerateValidCartItem(Guid cartId)
    {
        var faker = new Faker();
        return new CartItem
        {
            Id = faker.Random.Guid(),
            CartId = cartId,
            ProductId = faker.Random.Guid(),
            Quantity = faker.Random.Int(1, 10)
        };
    }

    /// <summary>
    /// Generates a valid user ID.
    /// </summary>
    /// <returns>A valid user ID (Guid).</returns>
    public static Guid GenerateValidUserId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid product ID.
    /// </summary>
    /// <returns>A valid product ID (Guid).</returns>
    public static Guid GenerateValidProductId()
    {
        return new Faker().Random.Guid();
    }

    /// <summary>
    /// Generates a valid quantity.
    /// </summary>
    /// <returns>A valid quantity (positive integer).</returns>
    public static int GenerateValidQuantity()
    {
        return new Faker().Random.Int(1, 100);
    }

    /// <summary>
    /// Generates an invalid user ID (empty Guid).
    /// </summary>
    /// <returns>An invalid user ID.</returns>
    public static Guid GenerateInvalidUserId()
    {
        return Guid.Empty;
    }

    /// <summary>
    /// Generates an invalid quantity (zero or negative).
    /// </summary>
    /// <returns>An invalid quantity.</returns>
    public static int GenerateInvalidQuantity()
    {
        return new Faker().Random.Int(-100, 0);
    }
}

