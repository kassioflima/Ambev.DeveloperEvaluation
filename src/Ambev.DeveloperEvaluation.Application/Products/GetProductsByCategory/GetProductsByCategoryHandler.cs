using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Handler for processing GetProductsByCategoryQuery requests
/// </summary>
public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetProductsByCategoryQueryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var query = _productRepository.GetQueryable()
            .Where(p => p.Category == request.Category);

        query = QueryHelper.ApplyOrdering(query, request.Order);

        var page = Math.Max(1, request.Page);
        var size = Math.Max(1, Math.Min(100, request.Size));

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        var products = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult
        {
            Data = _mapper.Map<List<GetProducts.GetProductItemResult>>(products),
            TotalItems = totalCount,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }
}

