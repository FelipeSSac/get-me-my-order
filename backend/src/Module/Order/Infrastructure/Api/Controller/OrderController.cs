using Microsoft.AspNetCore.Mvc;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Mapper;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Api.Controller.Response;

namespace Order.Infrastructure.Api.Controller;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly ICreateOrderUseCase _createOrderUseCase;

    public OrderController(ICreateOrderUseCase createOrderUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
    }

    [HttpGet]
    public IActionResult GetOrder()
    {
        return Ok("Hello");
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        OrderEntity order = await _createOrderUseCase.Execute(request);

        return Created($"/orders/{order.GetId()}", order.ToResponse());
    }
}