using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Result model for GetUsers operation
/// </summary>
public class GetUsersResult
{
    /// <summary>
    /// List of users
    /// </summary>
    public List<GetUserItemResult> Data { get; set; } = new();

    /// <summary>
    /// Total number of items
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// User item in the list
/// </summary>
public class GetUserItemResult
{
    /// <summary>
    /// The unique identifier of the user
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
    public UserNameDto? Name { get; set; }

    /// <summary>
    /// The user's address
    /// </summary>
    public UserAddressDto? Address { get; set; }

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

/// <summary>
/// DTO for user name
/// </summary>
public class UserNameDto
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}

/// <summary>
/// DTO for user address
/// </summary>
public class UserAddressDto
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public UserGeolocationDto? Geolocation { get; set; }
}

/// <summary>
/// DTO for geolocation
/// </summary>
public class UserGeolocationDto
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}

