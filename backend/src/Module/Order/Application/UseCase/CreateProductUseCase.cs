using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class CreateProductUseCase : ICreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductUseCase> _logger;

    public CreateProductUseCase(
        IProductRepository productRepository,
        ILogger<CreateProductUseCase> logger
    ) {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductEntity> Execute(CreateProductRequest request)
    {
        _logger.LogInformation("[CreateProductUseCase::Execute] Creating product {ProductName} with value {Value} {Currency}",
            request.Name, request.Value, request.Currency);

        try
        {
            ProductEntity product = ProductEntity.Create(
                null,
                request.Name,
                Money.Create(request.Value, request.Currency)
            );

            await _productRepository.AddAsync(product);

            _logger.LogInformation("[CreateProductUseCase::Execute] Product {ProductId} created successfully with name {ProductName}",
                product.GetId(), request.Name);

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CreateProductUseCase::Execute] Error creating product {ProductName}", request.Name);
            throw;
        }
    }
}