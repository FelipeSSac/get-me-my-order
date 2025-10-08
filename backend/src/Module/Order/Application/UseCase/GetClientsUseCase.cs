using Microsoft.Extensions.Logging;
using Order.Application.DTO;
using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;

namespace Order.Application.UseCase;

public class GetClientsUseCase : IGetClientsUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger _logger;

    public GetClientsUseCase(
        IClientRepository clientRepository,
        ILogger<GetClientsUseCase> logger)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }
    
    public async Task<PaginatedResult<ClientEntity>> Execute(int page, int pageSize)
    {
        _logger.LogInformation("[GetClientsUseCase::Execute] Fetching orders - Page: {Page}, PageSize: {PageSize}",
            page, pageSize);

        if (page < 1)
        {
            _logger.LogWarning("[GetClientsUseCase::Execute] Invalid page number: {Page}", page);
            throw new ArgumentException("Page must be greater than 0", nameof(page));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("[GetClientsUseCase::Execute] Invalid page size: {PageSize}", pageSize);
            throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));
        }

        var (items, totalCount) = await _clientRepository.GetPaginatedAsync(page, pageSize);

        var itemsList = items.ToList();

        _logger.LogInformation("[GetClientsUseCase::Execute] Retrieved {Count} clients out of {TotalCount} total", itemsList.Count, totalCount);

        return new PaginatedResult<ClientEntity>(itemsList, page, pageSize, totalCount);
    }
}