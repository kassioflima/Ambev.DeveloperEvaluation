using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Handler for processing GetUsersQuery requests
/// </summary>
public class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetUsersHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetUsersQuery request
    /// </summary>
    /// <param name="request">The GetUsers query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of users with pagination info</returns>
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _userRepository.GetQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            query = QueryHelper.ApplyStringFilter(query, "Email", request.Email);
        }

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            query = QueryHelper.ApplyStringFilter(query, "Username", request.Username);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(u => u.Status.ToString() == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(u => u.Role.ToString() == request.Role);
        }

        // Apply ordering
        query = QueryHelper.ApplyOrdering(query, request.Order);

        // Apply pagination
        var page = Math.Max(1, request.Page);
        var size = Math.Max(1, Math.Min(100, request.Size)); // Limit to 100 items per page

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)size);

        var users = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        var result = new GetUsersResult
        {
            Data = _mapper.Map<List<GetUserItemResult>>(users),
            TotalItems = totalCount,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return result;
    }
}

