using Order.Domain.Entity;
using Order.Infrastructure.Api.Controller.Request;

namespace Order.Application.UseCase.Interface;

public interface ICreateProductUseCase
{
    Task<ProductEntity> Execute(CreateProductRequest request);
}