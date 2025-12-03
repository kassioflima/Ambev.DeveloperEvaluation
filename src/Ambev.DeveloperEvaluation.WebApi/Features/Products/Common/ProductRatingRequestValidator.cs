using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

/// <summary>
/// Shared validator for ProductRatingRequest
/// </summary>
public class ProductRatingRequestValidator : AbstractValidator<ProductRatingRequest>
{
    public ProductRatingRequestValidator()
    {
        RuleFor(x => x.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5");

        RuleFor(x => x.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to 0");
    }
}

