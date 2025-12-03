namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

/// <summary>
/// Shared DTO for cart product items
/// </summary>
public class CartProductResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

