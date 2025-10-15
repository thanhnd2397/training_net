namespace Training.Application.IUseCases;

public interface IUserUseCase
{
    Task<BaseResponse<int>?> CreateUser(LoginRequest? request);
}