using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a shopping cart in the system
/// </summary>
public class Cart : BaseEntity
{
    /// <summary>
    /// Gets or sets the user ID (external identity)
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the cart date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the products in the cart
    /// </summary>
    public List<CartItem> Products { get; set; } = new();

    /// <summary>
    /// Gets or sets the date and time when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the cart
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Cart class
    /// </summary>
    public Cart()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Date = DateTime.UtcNow;
    }
}

/// <summary>
/// Represents an item in a shopping cart
/// </summary>
public class CartItem
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the product ID (external identity)
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the cart navigation property
    /// </summary>
    public Cart? Cart { get; set; }

    /// <summary>
    /// Initializes a new instance of the CartItem class
    /// </summary>
    public CartItem()
    {
        Id = Guid.NewGuid();
    }
}

