using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsQuery : IRequest<GetCartsResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}

