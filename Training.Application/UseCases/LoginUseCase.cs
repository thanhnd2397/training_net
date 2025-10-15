using Training.Application.Common;
using Training.Application.Common.Exceptions;
using Training.Application.Common.Interfaces;
using Training.Application.Common.Models;
using Training.Application.Common.Utils;
using Training.Application.Dtos.Request;
using Training.Application.Dtos.Response;
using Training.Application.IRepositories;
using Training.Application.IUseCases;

namespace Training.Application.UseCases;

public class LoginUseCase(IUserRepository userRepository, IJwtService jwtService, IMessageService messageService)
    : ILoginUseCase
{
    public async Task<BaseResponse<LoginResponse>?> ExecuteAsync(LoginRequest? request)
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