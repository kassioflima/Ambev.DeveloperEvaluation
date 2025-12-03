using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductResult> Products { get; set; } = new();
}

public class CartProductResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

