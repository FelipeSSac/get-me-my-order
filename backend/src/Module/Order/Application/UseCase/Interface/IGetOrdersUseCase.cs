using Order.Application.DTO;
using Order.Domain.Entity;
using Order.Domain.Enum;

namespace Order.Application.UseCase.Interface;

public interface IGetOrdersUseCase
{
    Task<PaginatedResult<OrderEntity>> Execute(int page, int pageSize, OrderStatus? status = null);
}