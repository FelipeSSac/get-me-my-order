using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class ClientEntity
{
    private Guid? Id { get; }
    private PersonName Name { get; }
    private Email Email { get; }
    private DateTime CreatedAt { get; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; } = DateTime.UtcNow;

    private ClientEntity() {}

    private ClientEntity(Guid? id, PersonName name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static ClientEntity Create(Guid? id, PersonName name, Email email)
    {
        return new ClientEntity(id, name, email);
    }

}