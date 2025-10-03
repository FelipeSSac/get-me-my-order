using Order.Domain.Entity;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Response;

namespace Order.Infrastructure.Api.Controller.Mapper;

public static class ProductMapper
{
    public static ProductResponse ToResponse(this ProductEntity product)
    {
        Money value = product.GetValue();

        return new ProductResponse(
            product.GetId() ?? Guid.Empty,
            product.GetName(),
            value.GetAmount(),
            value.GetCurrency()
        );
    }
}