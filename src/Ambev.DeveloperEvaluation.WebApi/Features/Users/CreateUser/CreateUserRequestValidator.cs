using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Validator for CreateUserRequest that defines validation rules for user creation.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Username: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Must be one of: Active, Inactive, Suspended
    /// - Role: Must be one of: Customer, Manager, Admin
    /// </remarks>
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());
        RuleFor(user => user.Username).NotEmpty().Length(3, 50);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
        RuleFor(user => user.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => s == "Active" || s == "Inactive" || s == "Suspended")
            .WithMessage("Status must be one of: Active, Inactive, Suspended");
        RuleFor(user => user.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(r => r == "Customer" || r == "Manager" || r == "Admin")
            .WithMessage("Role must be one of: Customer, Manager, Admin");
    }
}