using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.TestData;

/// <summary>
/// Provides methods for generating test data for functional tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency across functional test cases.
/// </summary>
public static class FunctionalTestData
{
    #region User Test Data

    /// <summary>
    /// Generates a valid CreateUserCommand for functional tests
    /// </summary>
    public static CreateUserCommand GenerateValidCreateUserCommand()
    {
        var faker = new Faker();
        return new CreateUserCommand
        {
            Username = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            Password = $"Test@{faker.Random.Number(100, 999)}",
            Phone = $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}",
            Status = faker.PickRandom(UserStatus.Active, UserStatus.Suspended),
            Role = faker.PickRandom(UserRole.Customer, UserRole.Admin),
            Name = new UserName(faker.Name.FirstName(), faker.Name.LastName()),
            Address = new UserAddress(
                faker.Address.City(),
                faker.Address.StreetName(),
                faker.Random.Int(1, 9999),
                faker.Address.ZipCode(),
                new UserGeolocation(
                    faker.Address.Latitude().ToString("F6"),
                    faker.Address.Longitude().ToString("F6")))
        };
    }

    /// <summary>
    /// Generates a valid User entity for functional tests
    /// </summary>
    public static User GenerateValidUser()
    {
        var faker = new Faker();
        return new User
        {
            Id = Guid.NewGuid(),
            Username = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            Password = $"hashed_Test@{faker.Random.Number(100, 999)}",
            Phone = $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}",
            Status = faker.PickRandom(UserStatus.Active, UserStatus.Suspended),
            Role = faker.PickRandom(UserRole.Customer, UserRole.Admin),
            Name = new UserName(faker.Name.FirstName(), faker.Name.LastName()),
            Address = new UserAddress(
                faker.Address.City(),
                faker.Address.StreetName(),
                faker.Random.Int(1, 9999),
                faker.Address.ZipCode())
        };
    }

    #endregion

    #region Product Test Data

    /// <summary>
    /// Generates a valid CreateProductCommand for functional tests
    /// </summary>
    public static CreateProductCommand GenerateValidCreateProductCommand()
    {
        var faker = new Faker();
        return new CreateProductCommand
        {
            Title = faker.Commerce.ProductName(),
            Price = faker.Random.Decimal(0.01m, 10000.00m),
            Description = faker.Lorem.Paragraph(),
            Category = faker.Commerce.Categories(1)[0],
            Image = faker.Image.PicsumUrl(),
            Rating = new ProductRating(
                faker.Random.Decimal(0, 5),
                faker.Random.Int(0, 1000))
        };
    }

    /// <summary>
    /// Generates a valid Product entity for functional tests
    /// </summary>
    public static Product GenerateValidProduct()
    {
        var faker = new Faker();
        return new Product
        {
            Id = Guid.NewGuid(),
            Title = faker.Commerce.ProductName(),
            Price = faker.Random.Decimal(0.01m, 10000.00m),
            Description = faker.Lorem.Paragraph(),
            Category = faker.Commerce.Categories(1)[0],
            Image = faker.Image.PicsumUrl(),
            Rating = new ProductRating(
                faker.Random.Decimal(0, 5),
                faker.Random.Int(0, 1000)),
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion

    #region Cart Test Data

    /// <summary>
    /// Generates a valid CreateCartCommand for functional tests
    /// </summary>
    public static CreateCartCommand GenerateValidCreateCartCommand(int productCount = 3)
    {
        var faker = new Faker();
        var products = new List<CartProductCommand>();
        
        for (int i = 0; i < productCount; i++)
        {
            products.Add(new CartProductCommand
            {
                ProductId = Guid.NewGuid(),
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return new CreateCartCommand
        {
            UserId = Guid.NewGuid(),
            Date = faker.Date.Recent(),
            Products = products
        };
    }

    /// <summary>
    /// Generates a valid Cart entity for functional tests
    /// </summary>
    public static Cart GenerateValidCart(Guid? cartId = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cart = new Cart
        {
            Id = cartId ?? Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Date = faker.Date.Recent(),
            CreatedAt = DateTime.UtcNow
        };

        for (int i = 0; i < itemCount; i++)
        {
            cart.Products.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = Guid.NewGuid(),
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return cart;
    }

    #endregion
}

