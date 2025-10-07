using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;
using Microsoft.Extensions.Logging;

namespace Order.Application.UseCase;

public class CreateClientUseCase : ICreateClientUseCase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<CreateClientUseCase> _logger;

    public CreateClientUseCase(
        IClientRepository clientRepository,
        ILogger<CreateClientUseCase> logger
    ) {
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public async Task<ClientEntity> Execute(CreateClientRequest request)
    {
        _logger.LogInformation("[CreateClientUseCase::Execute] Creating client with email {Email}", request.Email);

        try
        {
            ClientEntity? clientExists = await _clientRepository.GetByEmailAsync(request.Email);

            if (clientExists != null)
            {
                _logger.LogWarning("[CreateClientUseCase::Execute] Client with email {Email} already exists", request.Email);
                throw new Exception($"Client with email {request.Email} already exists");
            }

            ClientEntity client = ClientEntity.Create(
                null,
                PersonName.Create(request.FirstName,  request.LastName),
                Email.Create(request.Email)
            );

            await _clientRepository.AddAsync(client);

            _logger.LogInformation("[CreateClientUseCase::Execute] Client {ClientId} created successfully with email {Email}",
                client.GetId(), request.Email);

            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CreateClientUseCase::Execute] Error creating client with email {Email}", request.Email);
            throw;
        }
    }
}