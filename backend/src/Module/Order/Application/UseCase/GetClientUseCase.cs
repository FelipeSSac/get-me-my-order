using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class GetClientUseCase : IGetClientUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<GetClientUseCase> _logger;

    public GetClientUseCase(
        IClientRepository clientRepository,
        ILogger<GetClientUseCase> logger)
    {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public async Task<ClientEntity?> Execute(string id)
    {
        _logger.LogInformation("[GetClientUseCase::Execute] Fetching client with id {ClientId}", id);

        bool isIdGuid = Guid.TryParse(id.ToString(), out Guid idGuid);

        if (!isIdGuid)
        {
            _logger.LogWarning("[GetClientUseCase::Execute] Invalid client id format: {ClientId}", id);
            throw new ArgumentException("Invalid client id");
        }

        var client = await _clientRepository.GetByIdAsync(idGuid);

        if (client == null)
        {
            _logger.LogWarning("[GetClientUseCase::Execute] Client {ClientId} not found", id);
        }
        else
        {
            _logger.LogInformation("[GetClientUseCase::Execute] Client {ClientId} found with email {Email}",
                id, client.GetEmail());
        }

        return client;
    }
}
