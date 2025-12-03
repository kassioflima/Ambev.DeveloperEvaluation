using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public record GetCartQuery : IRequest<GetCartResult>
{
    public Guid Id { get; }

    public GetCartQuery(Guid id)
    {
        Id = id;
    }
}

