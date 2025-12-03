using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.ValueObjects;

/// <summary>
/// Contains unit tests for Value Objects (UserName, UserAddress, UserGeolocation, ProductRating).
/// Tests cover value object creation, property validation, and immutability.
/// </summary>
public class ValueObjectsTests
{
    private readonly Faker _faker = new();

    #region UserName Tests

    /// <summary>
    /// Tests that UserName can be created with valid first and last names.
    /// </summary>
    [Fact(DisplayName = "UserName should be created with valid first and last names")]
    public void Given_ValidNames_When_CreatingUserName_Then_ShouldHaveValidProperties()
    {
        // Arrange
        var firstname = _faker.Name.FirstName();
        var lastname = _faker.Name.LastName();

        // Act
        var userName = new UserName(firstname, lastname);

        // Assert
        userName.Firstname.Should().Be(firstname);
        userName.Lastname.Should().Be(lastname);
    }

    /// <summary>
    /// Tests that UserName can be created with empty constructor.
    /// </summary>
    [Fact(DisplayName = "UserName should be created with empty constructor")]
    public void Given_EmptyConstructor_When_CreatingUserName_Then_ShouldHaveEmptyProperties()
    {
        // Arrange & Act
        var userName = new UserName();

        // Assert
        userName.Firstname.Should().BeEmpty();
        userName.Lastname.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that UserName properties can be set.
    /// </summary>
    [Fact(DisplayName = "UserName properties should be settable")]
    public void Given_UserName_When_PropertiesAreSet_Then_ShouldReflectChanges()
    {
        // Arrange
        var userName = new UserName();
        var firstname = _faker.Name.FirstName();
        var lastname = _faker.Name.LastName();

        // Act
        userName.Firstname = firstname;
        userName.Lastname = lastname;

        // Assert
        userName.Firstname.Should().Be(firstname);
        userName.Lastname.Should().Be(lastname);
    }

    #endregion

    #region UserAddress Tests

    /// <summary>
    /// Tests that UserAddress can be created with valid properties.
    /// </summary>
    [Fact(DisplayName = "UserAddress should be created with valid properties")]
    public void Given_ValidAddressData_When_CreatingUserAddress_Then_ShouldHaveValidProperties()
    {
        // Arrange
        var city = _faker.Address.City();
        var street = _faker.Address.StreetName();
        var number = _faker.Random.Int(1, 9999);
        var zipcode = _faker.Address.ZipCode();

        // Act
        var address = new UserAddress(city, street, number, zipcode);

        // Assert
        address.City.Should().Be(city);
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.Zipcode.Should().Be(zipcode);
        address.Geolocation.Should().BeNull();
    }

    /// <summary>
    /// Tests that UserAddress can be created with geolocation.
    /// </summary>
    [Fact(DisplayName = "UserAddress should support geolocation")]
    public void Given_AddressWithGeolocation_When_CreatingUserAddress_Then_ShouldHaveGeolocation()
    {
        // Arrange
        var city = _faker.Address.City();
        var street = _faker.Address.StreetName();
        var number = _faker.Random.Int(1, 9999);
        var zipcode = _faker.Address.ZipCode();
        var geolocation = new UserGeolocation(
            _faker.Address.Latitude().ToString("F6"),
            _faker.Address.Longitude().ToString("F6"));

        // Act
        var address = new UserAddress(city, street, number, zipcode, geolocation);

        // Assert
        address.Geolocation.Should().NotBeNull();
        address.Geolocation!.Lat.Should().Be(geolocation.Lat);
        address.Geolocation.Long.Should().Be(geolocation.Long);
    }

    /// <summary>
    /// Tests that UserAddress can be created with empty constructor.
    /// </summary>
    [Fact(DisplayName = "UserAddress should be created with empty constructor")]
    public void Given_EmptyConstructor_When_CreatingUserAddress_Then_ShouldHaveEmptyProperties()
    {
        // Arrange & Act
        var address = new UserAddress();

        // Assert
        address.City.Should().BeEmpty();
        address.Street.Should().BeEmpty();
        address.Number.Should().Be(0);
        address.Zipcode.Should().BeEmpty();
        address.Geolocation.Should().BeNull();
    }

    /// <summary>
    /// Tests that UserAddress properties can be set.
    /// </summary>
    [Fact(DisplayName = "UserAddress properties should be settable")]
    public void Given_UserAddress_When_PropertiesAreSet_Then_ShouldReflectChanges()
    {
        // Arrange
        var address = new UserAddress();
        var city = _faker.Address.City();
        var street = _faker.Address.StreetName();
        var number = _faker.Random.Int(1, 9999);
        var zipcode = _faker.Address.ZipCode();

        // Act
        address.City = city;
        address.Street = street;
        address.Number = number;
        address.Zipcode = zipcode;

        // Assert
        address.City.Should().Be(city);
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.Zipcode.Should().Be(zipcode);
    }

    #endregion

    #region UserGeolocation Tests

    /// <summary>
    /// Tests that UserGeolocation can be created with valid coordinates.
    /// </summary>
    [Fact(DisplayName = "UserGeolocation should be created with valid coordinates")]
    public void Given_ValidCoordinates_When_CreatingUserGeolocation_Then_ShouldHaveValidProperties()
    {
        // Arrange
        var lat = _faker.Address.Latitude().ToString("F6");
        var @long = _faker.Address.Longitude().ToString("F6");

        // Act
        var geolocation = new UserGeolocation(lat, @long);

        // Assert
        geolocation.Lat.Should().Be(lat);
        geolocation.Long.Should().Be(@long);
    }

    /// <summary>
    /// Tests that UserGeolocation can be created with empty constructor.
    /// </summary>
    [Fact(DisplayName = "UserGeolocation should be created with empty constructor")]
    public void Given_EmptyConstructor_When_CreatingUserGeolocation_Then_ShouldHaveEmptyProperties()
    {
        // Arrange & Act
        var geolocation = new UserGeolocation();

        // Assert
        geolocation.Lat.Should().BeEmpty();
        geolocation.Long.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that UserGeolocation properties can be set.
    /// </summary>
    [Fact(DisplayName = "UserGeolocation properties should be settable")]
    public void Given_UserGeolocation_When_PropertiesAreSet_Then_ShouldReflectChanges()
    {
        // Arrange
        var geolocation = new UserGeolocation();
        var lat = _faker.Address.Latitude().ToString("F6");
        var @long = _faker.Address.Longitude().ToString("F6");

        // Act
        geolocation.Lat = lat;
        geolocation.Long = @long;

        // Assert
        geolocation.Lat.Should().Be(lat);
        geolocation.Long.Should().Be(@long);
    }

    #endregion

    #region ProductRating Tests

    /// <summary>
    /// Tests that ProductRating can be created with valid rate and count.
    /// </summary>
    [Fact(DisplayName = "ProductRating should be created with valid rate and count")]
    public void Given_ValidRatingData_When_CreatingProductRating_Then_ShouldHaveValidProperties()
    {
        // Arrange
        var rate = _faker.Random.Decimal(0, 5);
        var count = _faker.Random.Int(0, 1000);

        // Act
        var rating = new ProductRating(rate, count);

        // Assert
        rating.Rate.Should().Be(rate);
        rating.Count.Should().Be(count);
    }

    /// <summary>
    /// Tests that ProductRating can be created with empty constructor.
    /// </summary>
    [Fact(DisplayName = "ProductRating should be created with empty constructor")]
    public void Given_EmptyConstructor_When_CreatingProductRating_Then_ShouldHaveDefaultProperties()
    {
        // Arrange & Act
        var rating = new ProductRating();

        // Assert
        rating.Rate.Should().Be(0);
        rating.Count.Should().Be(0);
    }

    /// <summary>
    /// Tests that ProductRating properties can be set.
    /// </summary>
    [Fact(DisplayName = "ProductRating properties should be settable")]
    public void Given_ProductRating_When_PropertiesAreSet_Then_ShouldReflectChanges()
    {
        // Arrange
        var rating = new ProductRating();
        var rate = _faker.Random.Decimal(0, 5);
        var count = _faker.Random.Int(0, 1000);

        // Act
        rating.Rate = rate;
        rating.Count = count;

        // Assert
        rating.Rate.Should().Be(rate);
        rating.Count.Should().Be(count);
    }

    /// <summary>
    /// Tests that ProductRating rate can be zero.
    /// </summary>
    [Fact(DisplayName = "ProductRating rate should allow zero")]
    public void Given_ProductRating_When_RateIsZero_Then_ShouldBeValid()
    {
        // Arrange & Act
        var rating = new ProductRating(0, 10);

        // Assert
        rating.Rate.Should().Be(0);
        rating.Count.Should().Be(10);
    }

    /// <summary>
    /// Tests that ProductRating count can be zero.
    /// </summary>
    [Fact(DisplayName = "ProductRating count should allow zero")]
    public void Given_ProductRating_When_CountIsZero_Then_ShouldBeValid()
    {
        // Arrange & Act
        var rating = new ProductRating(4.5m, 0);

        // Assert
        rating.Rate.Should().Be(4.5m);
        rating.Count.Should().Be(0);
    }

    #endregion
}

