using FluentValidation;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Core.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage("Stock is required")
            .GreaterThan(0).WithMessage("Stock must be greater than 0");
    }
}