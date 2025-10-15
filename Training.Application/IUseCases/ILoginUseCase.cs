namespace Training.Application.IUseCases;

public interface ILoginUseCase
{
    Task<BaseResponse<LoginResponse>?> LoginAsync(LoginRequest? request);
}