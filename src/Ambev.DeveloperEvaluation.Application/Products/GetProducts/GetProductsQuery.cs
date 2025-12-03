using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Query for retrieving a list of products with pagination, filtering and sorting
/// </summary>
public class GetProductsQuery : IRequest<GetProductsResult>
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
    /// Ordering string (e.g., "price desc, title asc")
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Filter by title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Filter by category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Filter by exact price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }
}

