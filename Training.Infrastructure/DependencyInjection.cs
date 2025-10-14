using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Training.Application.Common;
using Training.Infrastructure.Persistence;
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

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}