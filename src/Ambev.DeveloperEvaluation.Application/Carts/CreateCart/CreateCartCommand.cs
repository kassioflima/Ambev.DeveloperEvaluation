using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartCommand : IRequest<CreateCartResult>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProductCommand> Products { get; set; } = new();
}

public class CartProductCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

