using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    public CreateCartRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Products list cannot be empty");

        RuleForEach(x => x.Products)
            .SetValidator(new CartProductRequestValidator());
    }
}

