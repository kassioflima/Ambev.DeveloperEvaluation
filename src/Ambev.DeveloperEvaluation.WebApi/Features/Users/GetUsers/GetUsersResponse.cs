using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Response model for getting a list of users
/// </summary>
public class GetUsersResponse
{
    /// <summary>
    /// List of users
    /// </summary>
    public List<GetUserItemResponse> Data { get; set; } = new();

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
public class GetUserItemResponse
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

