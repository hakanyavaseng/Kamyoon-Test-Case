using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Core.Validators;

namespace ProductManagement.Core;

public static class ServiceRegistration
{
    public static void AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<ProductValidator>();
    }
}