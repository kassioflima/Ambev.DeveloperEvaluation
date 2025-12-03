using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public record DeleteCartCommand : IRequest<bool>
{
    public Guid Id { get; }

    public DeleteCartCommand(Guid id)
    {
        Id = id;
    }
}

