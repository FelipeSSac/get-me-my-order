using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Application.UseCase;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    
    public CreateProductUseCase(
        IProductRepository productRepository
    ) {
        _productRepository = productRepository;
    }
    
    public async Task<ProductEntity> Execute(CreateProductRequest request)
    {
        try
        {
            ProductEntity product = ProductEntity.Create(
                null,
                request.Name,
                Money.Create(request.Value, request.Currency)
            );

            await _productRepository.AddAsync(product);

            return product;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            throw;
        }
    }
}