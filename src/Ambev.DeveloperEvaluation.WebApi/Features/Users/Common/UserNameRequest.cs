namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

/// <summary>
/// Shared DTO for user name requests
/// </summary>
public class UserNameRequest
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
}

