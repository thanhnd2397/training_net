namespace Training.Application.IUseCases.Base;

public interface IUseCase<in TRequest, out TResponse>
{
    TResponse Execute(TRequest request);
}