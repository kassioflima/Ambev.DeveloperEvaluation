namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object representing a user's name with first and last name
/// </summary>
public class UserName
{
    /// <summary>
    /// Gets or sets the first name
    /// </summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name
    /// </summary>
    public string Lastname { get; set; } = string.Empty;

    public UserName()
    {
    }

    public UserName(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
}

