using Microsoft.AspNetCore.Mvc;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Mapper;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Infrastructure.Api.Controller;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly ICreateClientUseCase _createClientUseCase;

    public ClientController(ICreateClientUseCase createClientUseCase)
    {
        _createClientUseCase = createClientUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
    {
        ClientEntity client = await _createClientUseCase.Execute(request);
        
        return Created($"/clients/{client.GetId()}", client.ToResponse());
    }
}