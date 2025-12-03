namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object representing a user's address
/// </summary>
public class UserAddress
{
    /// <summary>
    /// Gets or sets the city
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the street
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the street number
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets the zipcode
    /// </summary>
    public string Zipcode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the geolocation
    /// </summary>
    public UserGeolocation? Geolocation { get; set; }

    public UserAddress()
    {
    }

    public UserAddress(string city, string street, int number, string zipcode, UserGeolocation? geolocation = null)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }
}

/// <summary>
/// Value object representing geolocation coordinates
/// </summary>
public class UserGeolocation
{
    /// <summary>
    /// Gets or sets the latitude
    /// </summary>
    public string Lat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the longitude
    /// </summary>
    public string Long { get; set; } = string.Empty;

    public UserGeolocation()
    {
    }

    public UserGeolocation(string lat, string @long)
    {
        Lat = lat;
        Long = @long;
    }
}

