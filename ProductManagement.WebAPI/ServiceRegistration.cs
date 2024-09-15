using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Core.Helpers;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Entities.Common;
using ProductManagement.Persistence.Contexts;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace ProductManagement.WebAPI;

public static class ServiceRegistration
{
    public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration,
        IHostBuilder builder, WebApplicationBuilder webApplicationBuilder)
    {
        ConfigureIdentity(services);
        ConfigureServiceLocator(services);
        ConfigureAuthentication(services, webApplicationBuilder);
    }

    public static void ConfigureAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));

        var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<Core.Options.TokenOptions>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience[0],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
            };
        });
    }

    public static void ConfigureServiceLocator(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        ServiceLocator.Initialize(serviceProvider);
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}