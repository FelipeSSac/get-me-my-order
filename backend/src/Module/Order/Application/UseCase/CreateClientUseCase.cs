using Order.Application.UseCase.Interface;
using Order.Domain.Entity;
using Order.Domain.Repository;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Application.UseCase;

public class CreateClientUseCase : ICreateClientUseCase
{
    private readonly IClientRepository _clientRepository;
    
    public CreateClientUseCase(
        IClientRepository clientRepository
    ) {
        _clientRepository = clientRepository;
    }

    public async Task<ClientEntity> Execute(CreateClientRequest request)
    {
        try
        {
            ClientEntity? clientExists = await _clientRepository.GetByEmailAsync(request.Email);
            
            if (clientExists != null)
                throw new Exception($"Client with email {request.Email} already exists");
            
            ClientEntity client = ClientEntity.Create(
                null,
                PersonName.Create(request.FirstName,  request.LastName),
                Email.Create(request.Email)
            );

            await _clientRepository.AddAsync(client);

            return client;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating client: {ex.Message}");
            throw;
        }
    }
}