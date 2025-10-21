using Microsoft.Extensions.DependencyInjection;
using Training.Application.Common.Mapper;
using Training.Application.UseCases;

namespace Training.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationProfile));
        
        services.AddScoped<IUserUseCase, UserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        return services;
    }
}