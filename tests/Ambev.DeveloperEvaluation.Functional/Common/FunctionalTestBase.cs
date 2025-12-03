using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.IoC;

namespace Ambev.DeveloperEvaluation.Functional.Common;

/// <summary>
/// Base class for functional tests providing common setup and teardown
/// </summary>
public abstract class FunctionalTestBase : IDisposable
{
    protected DefaultContext Context { get; private set; } = null!;
    protected IServiceProvider ServiceProvider { get; private set; } = null!;
    protected IMapper Mapper { get; private set; } = null!;

    protected FunctionalTestBase()
    {
        Setup();
    }

    private void Setup()
    {
        // Create in-memory database
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        // Register dependencies
        serviceCollection.AddAutoMapper(typeof(ApplicationLayer).Assembly);
        serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly
            );
        });

        // Register repositories
        serviceCollection.AddScoped<Domain.Repositories.IUserRepository, ORM.Repositories.UserRepository>();
        serviceCollection.AddScoped<Domain.Repositories.IProductRepository, ORM.Repositories.ProductRepository>();
        serviceCollection.AddScoped<Domain.Repositories.ICartRepository, ORM.Repositories.CartRepository>();

        // Register password hasher (mock implementation for tests)
        serviceCollection.AddScoped<Ambev.DeveloperEvaluation.Common.Security.IPasswordHasher, TestPasswordHasher>();
        serviceCollection.AddScoped<Ambev.DeveloperEvaluation.Common.Security.IJwtTokenGenerator, TestJwtTokenGenerator>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
        Context = ServiceProvider.GetRequiredService<DefaultContext>();
        Mapper = ServiceProvider.GetRequiredService<IMapper>();

        // Ensure database is created
        Context.Database.EnsureCreated();
    }

    /// <summary>
    /// Cleans up test data after each test
    /// </summary>
    protected async Task CleanupAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        Context.Products.RemoveRange(Context.Products);
        Context.Carts.RemoveRange(Context.Carts);
        Context.CartItems.RemoveRange(Context.CartItems);
        await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context?.Database?.EnsureDeleted();
        Context?.Dispose();
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

/// <summary>
/// Simple password hasher implementation for testing
/// </summary>
public class TestPasswordHasher : Ambev.DeveloperEvaluation.Common.Security.IPasswordHasher
{
    public string HashPassword(string password)
    {
        return $"hashed_{password}";
    }

    public bool VerifyPassword(string password, string hash)
    {
        // The hash should be in format "hashed_{password}"
        // So we need to check if the hash matches what we would hash for the given password
        var expectedHash = $"hashed_{password}";
        return hash == expectedHash;
    }
}

/// <summary>
/// Simple JWT token generator implementation for testing
/// </summary>
public class TestJwtTokenGenerator : Ambev.DeveloperEvaluation.Common.Security.IJwtTokenGenerator
{
    public string GenerateToken(Ambev.DeveloperEvaluation.Common.Security.IUser user)
    {
        // IUser only has Id, Username, and Role, so we use Username as identifier
        return $"test_token_{user.Id}_{user.Username}";
    }
}

