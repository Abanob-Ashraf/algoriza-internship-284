using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VezeetaApi.Domain.Services;
using VezeetaApi.Domain;
using VezeetaApi.Infrastructure.AutoMapperConfig;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthRepository>();
            services.AddScoped<ISendingEmailService, SendingEmailService>();

            services.AddScoped<IInitializeDefaultData, InitializeDefaultDataRepository>();
            services.AddHostedService<InitializeDefaultDataService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseRepository<>));

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));
        }
    }
}
