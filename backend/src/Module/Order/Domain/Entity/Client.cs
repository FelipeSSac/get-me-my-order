using Order.Domain.ValueObject;

namespace Order.Domain.Entity;

public class Client
{
    private Guid? Id { get; }
    private PersonName Name { get; }
    private Email Email { get; }
    private DateTime CreatedAt { get; } = DateTime.Now;
    private DateTime UpdatedAt { get; } = DateTime.Now;

    private Client(Guid? id, PersonName name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static Client Create(Guid? id, PersonName name, Email email)
    {
        return new Client(id, name, email);
    }

}