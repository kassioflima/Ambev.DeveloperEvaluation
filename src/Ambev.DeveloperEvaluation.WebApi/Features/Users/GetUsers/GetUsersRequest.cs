using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Request model for getting a list of users
/// </summary>
public class GetUsersRequest
{
    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    [FromQuery(Name = "_page")]
    public int? Page { get; set; }

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    [FromQuery(Name = "_size")]
    public int? Size { get; set; }

    /// <summary>
    /// Ordering string (e.g., "username asc, email desc")
    /// </summary>
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }

    /// <summary>
    /// Filter by email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Filter by username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Filter by status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by role
    /// </summary>
    public string? Role { get; set; }
}

