namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

/// <summary>
/// Shared DTO for user address responses
/// </summary>
public class UserAddressResponse
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public UserGeolocationResponse? Geolocation { get; set; }
}

/// <summary>
/// Shared DTO for geolocation responses
/// </summary>
public class UserGeolocationResponse
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;
}

