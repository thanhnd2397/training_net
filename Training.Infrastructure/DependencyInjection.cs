using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Training.Application.Common.Interfaces;
using Training.Application.IRepositories;
using Training.Application.IService;
using Training.Application.IUseCases;
using Training.Application.UseCases;
using Training.Infrastructure.Persistence;
using Training.Infrastructure.Repositories;
using Training.Infrastructure.Services;

namespace Training.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)))
            );
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<IMessageService, MessageService>();

            return services;
        }
    }
}