namespace Order.Application.UseCase.Interface;

public interface IProcessOrderUseCase
{
    Task Execute(string id);
}