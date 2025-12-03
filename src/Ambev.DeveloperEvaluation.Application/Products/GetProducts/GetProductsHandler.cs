using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Handler for processing GetProductsQuery requests
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = QueryHelper.ApplyStringFilter(query, "Title", request.Title);
        }

        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            query = QueryHelper.ApplyStringFilter(query, "Category", request.Category);
        }

        if (request.Price.HasValue)
        {
            query = query.Where(p => p.Price == request.Price.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        // Apply ordering
        query = QueryHelper.ApplyOrdering(query, request.Order);

        // Apply pagination
        var page = Math.Max(1, request.Page);
        var size = Math.Max(1, Math.Min(100, request.Size));

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        var result = new GetProductsResult
        {
            Data = _mapper.Map<List<GetProductItemResult>>(products),
            TotalItems = totalCount,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return result;
    }
}

