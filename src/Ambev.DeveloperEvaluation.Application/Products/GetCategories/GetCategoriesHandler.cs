using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Handler for processing GetCategoriesQuery requests
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResult>
{
    private readonly IProductRepository _productRepository;

    public GetCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetCategoriesResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _productRepository.GetCategoriesAsync(cancellationToken);
        return new GetCategoriesResult { Categories = categories };
    }
}

