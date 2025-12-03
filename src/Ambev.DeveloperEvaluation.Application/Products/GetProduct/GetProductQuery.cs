using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Query for retrieving a product by ID
/// </summary>
public record GetProductQuery : IRequest<GetProductResult>
{
    public Guid Id { get; }

    public GetProductQuery(Guid id)
    {
        Id = id;
    }
}

