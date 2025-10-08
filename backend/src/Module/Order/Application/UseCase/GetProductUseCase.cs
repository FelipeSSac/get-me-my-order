using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class GetProductUseCase : IGetProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductUseCase> _logger;

    public GetProductUseCase(
        IProductRepository productRepository,
        ILogger<GetProductUseCase> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductEntity?> Execute(string id)
    {
        _logger.LogInformation("[GetProductUseCase::Execute] Fetching product with id {ProductId}", id);

        bool isIdGuid = Guid.TryParse(id.ToString(), out Guid idGuid);

        if (!isIdGuid)
        {
            _logger.LogWarning("[GetProductUseCase::Execute] Invalid product id format: {ProductId}", id);
            throw new ArgumentException("Invalid product id");
        }

        var product = await _productRepository.GetByIdAsync(idGuid);

        if (product == null)
        {
            _logger.LogWarning("[GetProductUseCase::Execute] Product {ProductId} not found", id);
        }
        else
        {
            _logger.LogInformation("[GetProductUseCase::Execute] Product {ProductId} found with name {Name}",
                id, product.GetName());
        }

        return product;
    }
}
