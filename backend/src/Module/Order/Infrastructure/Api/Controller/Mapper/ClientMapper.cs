using Order.Domain.Entity;
using Order.Domain.ValueObject;
using Order.Infrastructure.Api.Controller.Request;
using Order.Infrastructure.Api.Controller.Response;

namespace Order.Infrastructure.Api.Controller.Mapper;

public static class ClientMapper
{
    public static ClientResponse ToResponse(this ClientEntity client)
    {
        PersonName name = client.GetName();
        
        return new ClientResponse(
            client.GetId() ?? Guid.Empty,
            name.GetFirstName(),
            name.GetLastName(),
            client.GetEmail().ToString(),
            client.GetCreatedAt(),
            client.GetUpdatedAt()
        );
    }
}