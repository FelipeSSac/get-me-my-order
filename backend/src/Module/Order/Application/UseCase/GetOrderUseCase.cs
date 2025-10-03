using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;

namespace Order.Application.UseCase;

public class GetOrderUseCase : IGetOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    
    public GetOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderEntity?> Execute(string id)
    {
        bool isIdGuid = Guid.TryParse(id.ToString(), out Guid idGuid);
        
        if (!isIdGuid)
            throw new ArgumentException("Invalid order id");
        
        return await _orderRepository.GetByIdAsync(idGuid);
    }
}