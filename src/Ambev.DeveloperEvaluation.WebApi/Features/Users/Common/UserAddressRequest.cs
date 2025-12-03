namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

/// <summary>
/// Shared DTO for user address requests
/// </summary>
public class UserAddressRequest
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public UserGeolocationRequest? Geolocation { get; set; }
}

/// <summary>
/// Shared DTO for geolocation requests
/// </summary>
public class UserGeolocationRequest
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}

