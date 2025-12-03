namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsResponse
{
    public List<GetCartItemResponse> Data { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class GetCartItemResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductItemResponse> Products { get; set; } = new();
}

public class CartProductItemResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

