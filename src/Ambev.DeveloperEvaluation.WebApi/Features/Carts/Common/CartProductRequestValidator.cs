using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

/// <summary>
/// Shared validator for CartProductRequest
/// </summary>
public class CartProductRequestValidator : AbstractValidator<CartProductRequest>
{
    public CartProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

