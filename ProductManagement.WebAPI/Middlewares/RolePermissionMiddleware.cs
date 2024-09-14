using Microsoft.AspNetCore.Identity;
using ProductManagement.Core.Attributes;
using ProductManagement.Core.Consts;
using ProductManagement.Core.Enums;
using ProductManagement.Domain.Entities;

namespace ProductManagement.WebAPI.Middlewares;

public class RolePermissionMiddleware : IMiddleware
{
    private readonly IServiceProvider _serviceProvider;

    public RolePermissionMiddleware(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var endpoint = context.GetEndpoint();
            var authorizeDefinition = endpoint?.Metadata.GetMetadata<AuthorizeDefinitionAttribute>();

            if (endpoint is not null)
            {
                var isAuthEndpoint = endpoint.Metadata.Any(m => m is AuthorizeDefinitionAttribute);
                if (!isAuthEndpoint)
                {
                    await next(context);
                    return;
                }
            }

            var userId = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var user = await userManager.FindByNameAsync(userId);
            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                if (authorizeDefinition.ActionType == ActionType.Updating ||
                    authorizeDefinition.ActionType == ActionType.Deleting)
                {
                    if (userRoles.Contains(RoleConsts.Admin))
                    {
                        await next(context);
                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await next(context);
        }
    }
}