using Training.Application.Common.Models;
using Training.Application.Dtos.Request;
using Training.Application.Dtos.Response;

namespace Training.Application.IUseCases;

public interface ILoginUseCase
{
    Task<BaseResponse<LoginResponse>?> ExecuteAsync(LoginRequest? request);
}