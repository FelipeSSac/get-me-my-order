using Order.Application.DTO;
using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetOrdersUseCase
{
    Task<PaginatedResult<OrderEntity>> Execute(int page, int pageSize, Domain.Enum.OrderStatus? status = null);
}