using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Application.UseCase.Interface;

public interface ICreateClientUseCase
{
    Task<ClientEntity> Execute(CreateClientRequest request);
}