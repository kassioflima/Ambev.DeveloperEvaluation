using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ambev.DeveloperEvaluation.WebApi",
                    Version = "v1",
                    Description = "API RESTful para gerenciamento de usuários, produtos e carrinhos de compras",
                    Contact = new OpenApiContact
                    {
                        Name = "Ambev Developer Evaluation",
                        Email = "developer@ambev.com.br"
                    }
                });

                // Incluir comentários XML
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            
            // Apply database migrations on startup
            ApplyMigrations(app);
            
            // Exception middleware should be at the beginning to catch all exceptions
            app.UseMiddleware<ValidationExceptionMiddleware>();
            
            // Enable Swagger in Development and Docker environments
            if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Applies pending database migrations automatically on application startup
    /// </summary>
    /// <param name="app">The WebApplication instance</param>
    private static void ApplyMigrations(WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DefaultContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Checking for pending database migrations...");

            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Found {Count} pending migration(s): {Migrations}", 
                    pendingMigrations.Count, 
                    string.Join(", ", pendingMigrations));

                logger.LogInformation("Applying database migrations...");
                context.Database.Migrate();
                
                logger.LogInformation("Database migrations applied successfully");
            }
            else
            {
                logger.LogInformation("Database is up to date. No pending migrations.");
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while applying database migrations");
            throw; // Re-throw to prevent application from starting with invalid database state
        }
    }
}
