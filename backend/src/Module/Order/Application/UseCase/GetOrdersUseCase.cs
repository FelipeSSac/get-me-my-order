using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Microsoft.Extensions.Logging;
using Order.Domain.Enum;

namespace Order.Application.UseCase;

public class GetOrdersUseCase : IGetOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrdersUseCase> _logger;

    public GetOrdersUseCase(
        IOrderRepository orderRepository,
        ILogger<GetOrdersUseCase> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<PaginatedResult<OrderEntity>> Execute(int page, int pageSize, OrderStatus? status = null)
    {
        _logger.LogInformation("[GetOrdersUseCase::Execute] Fetching orders - Page: {Page}, PageSize: {PageSize}, Status: {Status}",
            page, pageSize, status?.ToString() ?? "All");

        if (page < 1)
        {
            _logger.LogWarning("[GetOrdersUseCase::Execute] Invalid page number: {Page}", page);
            throw new ArgumentException("Page must be greater than 0", nameof(page));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("[GetOrdersUseCase::Execute] Invalid page size: {PageSize}", pageSize);
            throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));
        }

        var (items, totalCount) = await _orderRepository.GetPaginatedAsync(page, pageSize, status);

        var itemsList = items.ToList();

        _logger.LogInformation("[GetOrdersUseCase::Execute] Retrieved {Count} orders out of {TotalCount} total", itemsList.Count, totalCount);

        return new PaginatedResult<OrderEntity>(itemsList, page, pageSize, totalCount);
    }
}