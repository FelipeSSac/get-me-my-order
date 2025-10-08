using Microsoft.AspNetCore.Mvc;
using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Mapper;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Infrastructure.Api.Controller;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly ICreateProductUseCase _createProductUseCase;
    private readonly IGetProductUseCase _getProductUseCase;
    private readonly IGetProductsUseCase _getProductsUseCase;

    public ProductController(
        ICreateProductUseCase createProductUseCase,
        IGetProductUseCase getProductUseCase,
        IGetProductsUseCase getProductsUseCase)
    {
        _createProductUseCase = createProductUseCase;
        _getProductUseCase = getProductUseCase;
        _getProductsUseCase = getProductsUseCase;
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        ProductEntity product = await _createProductUseCase.Execute(request);

        return Created($"/products/{product.GetId()}", product.ToResponse());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] string id)
    {
        ProductEntity? product = await _getProductUseCase.Execute(id);

        return Ok(product?.ToResponse());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        PaginatedResult<ProductEntity> result = await _getProductsUseCase.Execute(page, pageSize);

        return Ok(result.ToResponse(o => o.ToResponse()));
    }
}