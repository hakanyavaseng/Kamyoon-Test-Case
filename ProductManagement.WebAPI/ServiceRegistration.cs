using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Core.Helpers;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Entities.Common;
using ProductManagement.Persistence.Contexts;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;

namespace ProductManagement.WebAPI;

public static class ServiceRegistration
{
    public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration,
        IHostBuilder builder, WebApplicationBuilder webApplicationBuilder)
    {
        ConfigureLoggerService(services, configuration, builder);
        ConfigureIdentity(services);
        ConfigureServiceLocator(services);
        ConfigureAuthentication(services, webApplicationBuilder);
    }

    public static void ConfigureAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));

        Core.Options.TokenOptions? tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<Core.Options.TokenOptions>();

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

    public static void ConfigureLoggerService(this IServiceCollection services, IConfiguration configuration,
        IHostBuilder builder)
    {
        var columOptions = new ColumnOptions()
        {
            AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "RemoteIpAddress", DataType = SqlDbType.NVarChar, DataLength = 50 },
                new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 200 }
            }
        };

        Logger logConfig = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(configuration.GetConnectionString("DefaultConnection"), "Logs",
                autoCreateSqlTable: true,
                columnOptions: columOptions)
            .MinimumLevel.Warning()
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.UseSerilog(logConfig);

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
}