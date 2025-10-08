using Order.Application.DTO;
using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetProductsUseCase
{
    Task<PaginatedResult<ProductEntity>> Execute(int page, int pageSize);
}