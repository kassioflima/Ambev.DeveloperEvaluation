using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateCartCommand using the Bogus library.
/// </summary>
public static class CreateCartHandlerTestData
{
    private static readonly Faker<CreateCartCommand> CreateCartCommandFaker = new Faker<CreateCartCommand>()
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Date, f => f.Date.Recent())
        .RuleFor(c => c.Products, f => GenerateCartProducts(f, f.Random.Int(1, 5)));

    private static List<CartProductCommand> GenerateCartProducts(Faker faker, int count)
    {
        var products = new List<CartProductCommand>();
        for (int i = 0; i < count; i++)
        {
            products.Add(new CartProductCommand
            {
                ProductId = faker.Random.Guid(),
                Quantity = faker.Random.Int(1, 10)
            });
        }
        return products;
    }

    /// <summary>
    /// Generates a valid CreateCartCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateCartCommand with randomly generated data.</returns>
    public static CreateCartCommand GenerateValidCommand()
    {
        return CreateCartCommandFaker.Generate();
    }
}

