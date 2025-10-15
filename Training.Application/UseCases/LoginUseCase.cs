namespace Training.Application.UseCases;

public class LoginUseCase(IUserRepository userRepository, IJwtService jwtService, IMessageService messageService)
    : ILoginUseCase
{
    public async Task<BaseResponse<LoginResponse>?> LoginAsync(LoginRequest? request)
    {
        if (request == null)
            throw new BadRequestException("InvalidRequest");

        var user = await userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
            throw new BadRequestException("MSG_E001");

        if (!EncryptUtil.VerifyPassword(request.Password, user.Password))
            throw new BadRequestException("MSG_E001");

        var token = jwtService.GenerateToken(user.UserName, user.Role?.RoleName ?? "User");

        return new BaseResponse<LoginResponse>
        {
            Message = messageService.GetMessage("MSG_S001"),
            Data = new LoginResponse
            {
                Token = token,
            }
        };
    }
}