using UserManagement.Application.Interfaces;
using UserManagement.Application.Mapping;
using UserManagement.Application.Services;
using UserManagement.Infrastructure.Logger;
using UserManagement.Infrastructure.Repository;

namespace UserManagement.Api.Configuration
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                  AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<Application.Logger.ILogger, SerilogLogger>();

            return services;
        }
    }
}
