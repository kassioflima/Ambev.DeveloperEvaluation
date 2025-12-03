using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Functional.Common;
using Ambev.DeveloperEvaluation.Functional.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Auth;

/// <summary>
/// Functional tests for user authentication flow
/// Tests the complete flow from authentication command to token generation
/// </summary>
public class AuthenticateUserFunctionalTests : FunctionalTestBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public AuthenticateUserFunctionalTests()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
        _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    }

    /// <summary>
    /// Tests that a complete authentication flow works correctly
    /// </summary>
    [Fact(DisplayName = "Given valid credentials When authenticating Then authentication succeeds")]
    public async Task AuthenticateUser_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var createUserCommand = FunctionalTestData.GenerateValidCreateUserCommand();
        createUserCommand.Status = Domain.Enums.UserStatus.Active; // User must be active to authenticate
        await CleanupAsync();

        // Create user first
        var createdUser = await _mediator.Send(createUserCommand, CancellationToken.None);

        // Get the persisted user to see the hashed password
        var persistedUser = await _userRepository.GetByEmailAsync(createUserCommand.Email, CancellationToken.None);
        persistedUser.Should().NotBeNull();

        var authenticateCommand = new AuthenticateUserCommand
        {
            Email = createUserCommand.Email,
            Password = createUserCommand.Password // This is the plain password
        };

        // Act
        var result = await _mediator.Send(authenticateCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(createUserCommand.Email);
    }

    /// <summary>
    /// Tests that authentication fails with invalid email
    /// </summary>
    [Fact(DisplayName = "Given invalid email When authenticating Then authentication fails")]
    public async Task AuthenticateUser_InvalidEmail_ThrowsException()
    {
        // Arrange
        var command = new AuthenticateUserCommand
        {
            Email = "nonexistent@example.com",
            Password = "Test@123"
        };
        await CleanupAsync();

        // Act
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    /// <summary>
    /// Tests that authentication fails with invalid password
    /// </summary>
    [Fact(DisplayName = "Given invalid password When authenticating Then authentication fails")]
    public async Task AuthenticateUser_InvalidPassword_ThrowsException()
    {
        // Arrange
        var createUserCommand = FunctionalTestData.GenerateValidCreateUserCommand();
        await CleanupAsync();

        // Create user first
        await _mediator.Send(createUserCommand, CancellationToken.None);

        var authenticateCommand = new AuthenticateUserCommand
        {
            Email = createUserCommand.Email,
            Password = "WrongPassword@123"
        };

        // Act
        var act = () => _mediator.Send(authenticateCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    /// <summary>
    /// Tests that authentication fails with invalid data (empty email/password)
    /// Note: The handler doesn't validate before checking credentials, so it throws UnauthorizedAccessException
    /// </summary>
    [Fact(DisplayName = "Given invalid authentication data When authenticating Then throws unauthorized exception")]
    public async Task AuthenticateUser_InvalidData_ThrowsUnauthorizedException()
    {
        // Arrange
        var command = new AuthenticateUserCommand(); // Empty command
        await CleanupAsync();

        // Act
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        // The handler checks credentials first, so it throws UnauthorizedAccessException
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}

