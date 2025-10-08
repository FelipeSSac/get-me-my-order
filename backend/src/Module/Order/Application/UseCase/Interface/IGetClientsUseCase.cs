using Order.Application.DTO;
using Order.Domain.Entity;

namespace Order.Application.UseCase.Interface;

public interface IGetClientsUseCase
{
    Task<PaginatedResult<ClientEntity>> Execute(int page, int pageSize);
}