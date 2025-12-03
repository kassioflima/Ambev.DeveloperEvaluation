using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Request model for updating a user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The user's password (optional, only if updating password)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// The user's name
    /// </summary>
    public UserNameRequest? Name { get; set; }

    /// <summary>
    /// The user's address
    /// </summary>
    public UserAddressRequest? Address { get; set; }

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

