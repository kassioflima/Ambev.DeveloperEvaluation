namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// Shared DTO for product rating requests
/// </summary>
public class ProductRatingRequest
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}

