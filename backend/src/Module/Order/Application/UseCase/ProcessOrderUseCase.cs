using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;

namespace Order.Application.UseCase;

public class ProcessOrderUseCase : IProcessOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly int _processingTimeInMs = 5000;
    
    public ProcessOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Execute(string id)
    {
        OrderEntity? order = await _orderRepository.GetByIdAsync(new Guid(id));
        
        if (order == null)
            throw new Exception("Order not found!");

        order = await SetOrderAsProcessing(order);

        Thread.Sleep(_processingTimeInMs);
        
        await SetOrderAsDone(order);
    }

    private async Task<OrderEntity> SetOrderAsProcessing(OrderEntity order)
    {
        order = order.SetAsProcessing();
        await _orderRepository.UpdateAsync(order);

        return order;
    }

    private async Task<OrderEntity> SetOrderAsDone(OrderEntity order)
    {
        order = order.SetAsDone();
        await _orderRepository.UpdateAsync(order);
        
        return order;
    }
}