namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// Shared DTO for product rating responses
/// </summary>
public class ProductRatingResponse
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}

