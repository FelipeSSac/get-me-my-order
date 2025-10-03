using Microsoft.AspNetCore.Mvc;
using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Enum;
using Order.Infrastructure.Api.Controller.Mapper;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Api.Controller.Response;

namespace Order.Infrastructure.Api.Controller;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly ICreateOrderUseCase _createOrderUseCase;
    private readonly IGetOrderUseCase _getOrderUseCase;
    private readonly IGetOrdersUseCase _getOrdersUseCase;

    public OrderController(
        ICreateOrderUseCase createOrderUseCase,
        IGetOrderUseCase getOrderUseCase,
        IGetOrdersUseCase getOrdersUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getOrderUseCase = getOrderUseCase;
        _getOrdersUseCase = getOrdersUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        OrderEntity order = await _createOrderUseCase.Execute(request);

        return Created($"/orders/{order.GetId()}", order.ToResponse());
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] string id)
    {
        OrderEntity? order = await _getOrderUseCase.Execute(id);
        
        return Ok(order?.ToResponse());
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] OrderStatus? status = null)
    {
        PaginatedResult<OrderEntity> result = await _getOrdersUseCase.Execute(page, pageSize, status);

        return Ok(result.ToResponse(o => o.ToResponse()));
    }
}