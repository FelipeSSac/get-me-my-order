using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetOrderUseCase
{
    Task<OrderEntity?> Execute(string id);
}