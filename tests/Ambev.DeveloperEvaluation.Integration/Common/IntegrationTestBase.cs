using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Common;

/// <summary>
/// Base class for integration tests providing WebApplicationFactory and HTTP client
/// </summary>
public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly DefaultContext Context;
    private readonly IServiceScope _scope;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        
        // Get the context for cleanup - keep the scope alive
        _scope = Factory.Services.CreateScope();
        Context = _scope.ServiceProvider.GetRequiredService<DefaultContext>();
    }

    /// <summary>
    /// Creates JSON content from an object
    /// </summary>
    protected StringContent CreateJsonContent<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Reads JSON response content
    /// </summary>
    protected async Task<T?> ReadJsonResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
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
        _scope?.Dispose();
        Context?.Dispose();
        Client?.Dispose();
    }
}

/// <summary>
/// Custom WebApplicationFactory for integration tests
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            // Add test configuration for JWT
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:SecretKey", "TestSecretKeyForIntegrationTests12345678901234567890" }
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove the real database context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            // Use a fixed name so all requests share the same database
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "IntegrationTestDb");
            });

            // Ensure database is created
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            context.Database.EnsureCreated();
        });
    }
}

