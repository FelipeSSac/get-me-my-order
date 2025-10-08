using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetProductUseCase
{
    Task<ProductEntity?> Execute(string id);
}
