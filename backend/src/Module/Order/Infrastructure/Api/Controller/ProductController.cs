using Microsoft.AspNetCore.Mvc;
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
 
    public ProductController(ICreateProductUseCase createProductUseCase)
    {
        _createProductUseCase = createProductUseCase;
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        ProductEntity product = await _createProductUseCase.Execute(request);

        return Created($"/products/{product.GetId()}", product.ToResponse());
    }   
}