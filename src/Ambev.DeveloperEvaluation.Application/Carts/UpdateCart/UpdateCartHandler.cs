using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public UpdateCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingCart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingCart == null)
            throw new KeyNotFoundException($"Cart with ID {command.Id} not found");

        // Create a cart object with updated properties and products
        // The repository will handle clearing existing items and adding new ones
        var cartToUpdate = new Cart
        {
            Id = command.Id, // Use the command Id, not a new one
            UserId = command.UserId,
            Date = command.Date,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = existingCart.CreatedAt, // Preserve original creation date
            Products = command.Products.Select(p => new CartItem
            {
                CartId = command.Id,
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };

        var updatedCart = await _cartRepository.UpdateAsync(cartToUpdate, cancellationToken);
        return _mapper.Map<UpdateCartResult>(updatedCart);
    }
}

