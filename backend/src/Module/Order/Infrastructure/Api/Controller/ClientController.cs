using Microsoft.AspNetCore.Mvc;
using Order.Application.DTO;
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
    private readonly IGetClientUseCase _getClientUseCase;
    private readonly IGetClientsUseCase _getClientsUseCase;

    public ClientController(
        ICreateClientUseCase createClientUseCase,
        IGetClientUseCase getClientUseCase,
        IGetClientsUseCase getClientsUseCase)
    {
        _createClientUseCase = createClientUseCase;
        _getClientUseCase = getClientUseCase;
        _getClientsUseCase = getClientsUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
    {
        ClientEntity client = await _createClientUseCase.Execute(request);

        return Created($"/clients/{client.GetId()}", client.ToResponse());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetClient([FromRoute] string id)
    {
        ClientEntity? client = await _getClientUseCase.Execute(id);

        return Ok(client?.ToResponse());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        PaginatedResult<ClientEntity> result = await _getClientsUseCase.Execute(page, pageSize);

        return Ok(result.ToResponse(o => o.ToResponse()));
    }
}