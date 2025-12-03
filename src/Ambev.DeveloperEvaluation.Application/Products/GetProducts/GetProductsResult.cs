namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Result model for GetProducts operation
/// </summary>
public class GetProductsResult
{
    /// <summary>
    /// List of products
    /// </summary>
    public List<GetProductItemResult> Data { get; set; } = new();

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
/// Product item in the list
/// </summary>
public class GetProductItemResult
{
    /// <summary>
    /// The unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The product price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The product description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The product category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The product image URL
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The product rating
    /// </summary>
    public ProductRatingDto? Rating { get; set; }
}

/// <summary>
/// DTO for product rating
/// </summary>
public class ProductRatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}

