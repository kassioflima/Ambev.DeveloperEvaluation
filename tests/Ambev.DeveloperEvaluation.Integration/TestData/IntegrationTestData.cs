using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

/// <summary>
/// Provides methods for generating test data for integration tests using the Bogus library.
/// This class centralizes all test data generation for HTTP requests.
/// </summary>
public static class IntegrationTestData
{
    #region User Test Data

    /// <summary>
    /// Generates a valid CreateUserRequest for integration tests
    /// </summary>
    public static CreateUserRequest GenerateValidCreateUserRequest()
    {
        var faker = new Faker();
        return new CreateUserRequest
        {
            Username = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            Password = $"Test@{faker.Random.Number(100, 999)}",
            Phone = $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}",
            Status = faker.PickRandom("Active", "Suspended"),
            Role = faker.PickRandom("Customer", "Admin"),
            Name = new UserNameRequest
            {
                Firstname = faker.Name.FirstName(),
                Lastname = faker.Name.LastName()
            },
            Address = new UserAddressRequest
            {
                City = faker.Address.City(),
                Street = faker.Address.StreetName(),
                Number = faker.Random.Int(1, 9999),
                Zipcode = faker.Address.ZipCode(),
                Geolocation = new UserGeolocationRequest
                {
                    Lat = faker.Address.Latitude().ToString("F6"),
                    Long = faker.Address.Longitude().ToString("F6")
                }
            }
        };
    }

    /// <summary>
    /// Generates a valid UpdateUserRequest for integration tests
    /// </summary>
    public static UpdateUserRequest GenerateValidUpdateUserRequest()
    {
        var faker = new Faker();
        return new UpdateUserRequest
        {
            Username = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            Phone = $"+55{faker.Random.Number(11, 99)}{faker.Random.Number(100000000, 999999999)}",
            Status = "Active",
            Role = "Customer",
            Name = new UserNameRequest
            {
                Firstname = faker.Name.FirstName(),
                Lastname = faker.Name.LastName()
            }
        };
    }

    #endregion

    #region Product Test Data

    /// <summary>
    /// Generates a valid CreateProductRequest for integration tests
    /// </summary>
    public static CreateProductRequest GenerateValidCreateProductRequest()
    {
        var faker = new Faker();
        return new CreateProductRequest
        {
            Title = faker.Commerce.ProductName(),
            Price = faker.Random.Decimal(0.01m, 10000.00m),
            Description = faker.Lorem.Paragraph(),
            Category = faker.Commerce.Categories(1)[0],
            Image = faker.Image.PicsumUrl(),
            Rating = new ProductRatingRequest
            {
                Rate = faker.Random.Decimal(0, 5),
                Count = faker.Random.Int(0, 1000)
            }
        };
    }

    /// <summary>
    /// Generates a valid UpdateProductRequest for integration tests
    /// </summary>
    public static UpdateProductRequest GenerateValidUpdateProductRequest()
    {
        var faker = new Faker();
        return new UpdateProductRequest
        {
            Title = faker.Commerce.ProductName(),
            Price = faker.Random.Decimal(0.01m, 10000.00m),
            Description = faker.Lorem.Paragraph(),
            Category = faker.Commerce.Categories(1)[0],
            Image = faker.Image.PicsumUrl()
        };
    }

    #endregion

    #region Cart Test Data

    /// <summary>
    /// Generates a valid CreateCartRequest for integration tests
    /// </summary>
    public static CreateCartRequest GenerateValidCreateCartRequest(Guid userId, List<Guid> productIds, int productCount = 3)
    {
        var faker = new Faker();
        var products = new List<CartProductRequest>();
        
        // Use provided productIds or generate random Guids
        var productsToUse = productIds?.Take(productCount).ToList() ?? new List<Guid>();
        while (productsToUse.Count < productCount)
        {
            productsToUse.Add(Guid.NewGuid());
        }
        
        for (int i = 0; i < productCount; i++)
        {
            products.Add(new CartProductRequest
            {
                ProductId = productsToUse[i],
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return new CreateCartRequest
        {
            UserId = userId != Guid.Empty ? userId : Guid.NewGuid(),
            Date = faker.Date.Recent(),
            Products = products
        };
    }

    /// <summary>
    /// Generates a valid UpdateCartRequest for integration tests
    /// </summary>
    public static UpdateCartRequest GenerateValidUpdateCartRequest(Guid userId, List<Guid> productIds, int productCount = 2)
    {
        var faker = new Faker();
        var products = new List<CartProductRequest>();
        
        // Use provided productIds or generate random Guids
        var productsToUse = productIds?.Take(productCount).ToList() ?? new List<Guid>();
        while (productsToUse.Count < productCount)
        {
            productsToUse.Add(Guid.NewGuid());
        }
        
        for (int i = 0; i < productCount; i++)
        {
            products.Add(new CartProductRequest
            {
                ProductId = productsToUse[i],
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return new UpdateCartRequest
        {
            UserId = userId != Guid.Empty ? userId : Guid.NewGuid(),
            Date = faker.Date.Recent(),
            Products = products
        };
    }

    #endregion

    #region Auth Test Data

    /// <summary>
    /// Generates a valid AuthenticateUserRequest for integration tests
    /// </summary>
    public static AuthenticateUserRequest GenerateValidAuthenticateUserRequest(string email, string password)
    {
        return new AuthenticateUserRequest
        {
            Email = email,
            Password = password
        };
    }

    #endregion
}

