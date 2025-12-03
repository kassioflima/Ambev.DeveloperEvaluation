using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsHandler : IRequestHandler<GetCartsQuery, GetCartsResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartsHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<GetCartsResult> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var query = _cartRepository.GetQueryable();

        if (request.UserId.HasValue)
        {
            query = query.Where(c => c.UserId == request.UserId.Value);
        }

        if (request.MinDate.HasValue)
        {
            query = query.Where(c => c.Date >= request.MinDate.Value);
        }

        if (request.MaxDate.HasValue)
        {
            query = query.Where(c => c.Date <= request.MaxDate.Value);
        }

        query = QueryHelper.ApplyOrdering(query, request.Order);

        var page = Math.Max(1, request.Page);
        var size = Math.Max(1, Math.Min(100, request.Size));

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        var carts = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return new GetCartsResult
        {
            Data = _mapper.Map<List<GetCartItemResult>>(carts),
            TotalItems = totalCount,
            CurrentPage = page,
            TotalPages = totalPages
        };
    }
}

