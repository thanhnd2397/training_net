using AutoMapper;
using Training.Application.Common.Interfaces;

namespace Training.Application.UseCases;

public class UserUseCase(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, IMessageService messageService)
    : IUserUseCase
{
    public async Task<BaseResponse<int>?> CreateUser(CreateUserRequest? request)
    {   
        if (await userRepository.ExistsByUsernameAsync(request?.UserName))
        {
            throw new DuplicateException("ERR_USER_EXISTS"); // hoặc trả BaseResponse lỗi
        }
        
        var userId = await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var user = mapper.Map<User>(request);
            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return user.Id;
        });

        return new BaseResponse<int>
        {
            Message = messageService.GetMessage("S002"),
            Data = userId
        };
    }
    
}