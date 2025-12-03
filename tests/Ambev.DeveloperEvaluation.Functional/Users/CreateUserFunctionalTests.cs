using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Functional.Common;
using Ambev.DeveloperEvaluation.Functional.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Users;

/// <summary>
/// Functional tests for user creation flow
/// Tests the complete flow from command to database persistence
/// </summary>
public class CreateUserFunctionalTests : FunctionalTestBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public CreateUserFunctionalTests()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
        _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    }

    /// <summary>
    /// Tests that a complete user creation flow works correctly
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then user is created successfully")]
    public async Task CreateUser_ValidData_ReturnsSuccess()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateUserCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        // Verify user was persisted in database
        var persistedUser = await _userRepository.GetByIdAsync(result.Id);
        persistedUser.Should().NotBeNull();
        persistedUser!.Email.Should().Be(command.Email);
        persistedUser.Username.Should().Be(command.Username);
        persistedUser.Phone.Should().Be(command.Phone);
        persistedUser.Status.Should().Be(command.Status);
        persistedUser.Role.Should().Be(command.Role);
    }

    /// <summary>
    /// Tests that user creation fails with invalid data
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
    public async Task CreateUser_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateUserCommand(); // Empty command will fail validation
        await CleanupAsync();

        // Act
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that password is hashed before saving
    /// </summary>
    [Fact(DisplayName = "Given user creation When creating user Then password is hashed")]
    public async Task CreateUser_ValidData_PasswordIsHashed()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateUserCommand();
        var originalPassword = command.Password;
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedUser = await _userRepository.GetByIdAsync(result.Id);
        persistedUser.Should().NotBeNull();
        persistedUser!.Password.Should().NotBe(originalPassword);
        persistedUser.Password.Should().StartWith("hashed_");
    }

    /// <summary>
    /// Tests that duplicate email is not allowed
    /// </summary>
    [Fact(DisplayName = "Given duplicate email When creating user Then throws exception")]
    public async Task CreateUser_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateUserCommand();
        await CleanupAsync();

        // Create first user
        await _mediator.Send(command, CancellationToken.None);

        // Act - Try to create user with same email
        var act = () => _mediator.Send(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"User with email {command.Email} already exists");
    }

    /// <summary>
    /// Tests that user name and address are persisted correctly
    /// </summary>
    [Fact(DisplayName = "Given user with name and address When creating user Then name and address are persisted")]
    public async Task CreateUser_WithNameAndAddress_PersistsCorrectly()
    {
        // Arrange
        var command = FunctionalTestData.GenerateValidCreateUserCommand();
        await CleanupAsync();

        // Act
        var result = await _mediator.Send(command, CancellationToken.None);

        // Assert
        var persistedUser = await _userRepository.GetByIdAsync(result.Id);
        persistedUser.Should().NotBeNull();
        persistedUser!.Name.Should().NotBeNull();
        persistedUser.Name!.Firstname.Should().Be(command.Name!.Firstname);
        persistedUser.Name.Lastname.Should().Be(command.Name.Lastname);
        persistedUser.Address.Should().NotBeNull();
        persistedUser.Address!.City.Should().Be(command.Address!.City);
        persistedUser.Address.Street.Should().Be(command.Address.Street);
    }
}

