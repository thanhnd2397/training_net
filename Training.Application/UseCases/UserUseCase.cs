namespace Training.Application.UseCases;

public class UserUseCase : IUserUseCase
{
    public Task<BaseResponse<int>?> CreateUser(LoginRequest? request)
    {
        throw new NotImplementedException();
    }
}