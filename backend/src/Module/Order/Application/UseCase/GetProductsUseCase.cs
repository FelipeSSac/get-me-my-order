using Microsoft.Extensions.Logging;
using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;

namespace Order.Application.UseCase;

public class GetProductsUseCase : IGetProductsUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger _logger;

    public GetProductsUseCase(
        IProductRepository productRepository,
        ILogger<GetProductsUseCase> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }
    
    public async Task<PaginatedResult<ProductEntity>> Execute(int page, int pageSize)
    {
        _logger.LogInformation("[GetProductsUseCase::Execute] Fetching orders - Page: {Page}, PageSize: {PageSize}",
            page, pageSize);

        if (page < 1)
        {
            _logger.LogWarning("[GetProductsUseCase::Execute] Invalid page number: {Page}", page);
            throw new ArgumentException("Page must be greater than 0", nameof(page));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("[GetProductsUseCase::Execute] Invalid page size: {PageSize}", pageSize);
            throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));
        }

        var (items, totalCount) = await _productRepository.GetPaginatedAsync(page, pageSize);

        var itemsList = items.ToList();

        _logger.LogInformation("[GetProductsUseCase::Execute] Retrieved {Count} products out of {TotalCount} total", itemsList.Count, totalCount);

        return new PaginatedResult<ProductEntity>(itemsList, page, pageSize, totalCount);
    }
}