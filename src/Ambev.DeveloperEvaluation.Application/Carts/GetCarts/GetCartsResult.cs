namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsResult
{
    public List<GetCartItemResult> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class GetCartItemResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductItemResult> Products { get; set; } = new();
}

public class CartProductItemResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

