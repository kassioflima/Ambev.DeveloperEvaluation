using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CreateCartHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Date = command.Date,
            Products = command.Products.Select(p => new CartItem
            {
                CartId = Guid.NewGuid(), // Will be set correctly after cart is created
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        };
        
        // Set CartId for all items after cart is created
        foreach (var item in cart.Products)
        {
            item.CartId = cart.Id;
        }

        var createdCart = await _cartRepository.CreateAsync(cart, cancellationToken);
        return _mapper.Map<CreateCartResult>(createdCart);
    }
}

