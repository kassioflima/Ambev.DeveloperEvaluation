using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// API response model for CreateUser operation
/// </summary>
public class CreateUserResponse
{
    /// <summary>
    /// The unique identifier of the created user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The user's password (hashed)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The user's name
    /// </summary>
    public UserNameResponse? Name { get; set; }

    /// <summary>
    /// The user's address
    /// </summary>
    public UserAddressResponse? Address { get; set; }

    /// <summary>
    /// The user's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The user's status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The user's role
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
