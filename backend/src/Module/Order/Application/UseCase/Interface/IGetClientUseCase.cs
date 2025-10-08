using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetClientUseCase
{
    Task<ClientEntity?> Execute(string id);
}
