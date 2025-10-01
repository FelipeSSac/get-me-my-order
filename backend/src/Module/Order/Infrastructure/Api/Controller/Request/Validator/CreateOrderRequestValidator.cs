namespace Order.Infrastructure.Api.Controller.Request.Validator;

using FluentValidation;
using Order.Infrastructure.Api.Controller.Request;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("ClientId is required");

        RuleFor(x => x.ProductList)
            .NotEmpty()
            .WithMessage("At least one product is required");

        RuleForEach(x => x.ProductList).ChildRules(product =>
        {
            product.RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required");

            product.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        });
    }
}