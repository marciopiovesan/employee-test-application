using Employees.Application.Interfaces.Security;
using Employees.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
