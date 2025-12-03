using MediatR;
using Ambev.DeveloperEvaluation.Application.Users.GetUsers;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Query for retrieving a list of users with pagination, filtering and sorting
/// </summary>
public class GetUsersQuery : IRequest<GetUsersResult>
{
    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10)
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Ordering string (e.g., "username asc, email desc")
    /// </summary>
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

