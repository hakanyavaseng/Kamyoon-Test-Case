using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Core.Interfaces.UnitOfWork;
using ProductManagement.Persistence.Contexts;
using ProductManagement.Persistence.Repositories;
using ProductManagement.Persistence.Services;
using ProductManagement.Persistence.UnitOfWorks;

namespace ProductManagement.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureRepositories(services);
        ConfigureServices(services, configuration);
        ConfigureAutoMapper(services);
        ConfigureUnitOfWork(services);
    }
    
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceRegistration));
    }

    public static void ConfigureUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}