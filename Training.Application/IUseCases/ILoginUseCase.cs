namespace Training.Application.IUseCases;

public interface ILoginUseCase
{
    Task<BaseResponse<LoginResponse>?> ExecuteAsync(LoginRequest? request);
}