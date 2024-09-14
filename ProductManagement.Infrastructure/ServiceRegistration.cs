using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Infrastructure.Services;

namespace ProductManagement.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        
    }
    
    
}