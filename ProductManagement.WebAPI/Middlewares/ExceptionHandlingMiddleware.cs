using System.Net.Mime;
using System.Text.Json;
using ProductManagement.Core.DTOs.ApiResponses;
using SendGrid.Helpers.Errors.Model;

namespace ProductManagement.WebAPI.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private const string DefaultErrorMessage = "An error occurred while processing your request";

    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception exception) when (httpContext.RequestAborted.IsCancellationRequested)
        {
            const string message = "Request was cancelled";
            _logger.LogWarning(exception, message);

            await WriteErrorResponseAsync(httpContext, StatusCodes.Status499ClientClosedRequest, message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var errorResponse = CreateErrorResponse(exception, statusCode);

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var json = JsonSerializer.Serialize(errorResponse);
        _logger.LogError(exception, exception.Message);
        await httpContext.Response.WriteAsync(json);
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private ApiResponse<object> CreateErrorResponse(Exception exception, int statusCode)
    {
        var errorMessage = _env.IsProduction() ? DefaultErrorMessage : exception.Message;

        var errorDto = new ErrorDto(errorMessage);
        return ApiResponse<object>.Fail(errorDto, statusCode);
    }

    private async Task WriteErrorResponseAsync(HttpContext httpContext, int statusCode, string message)
    {
        var errorResponse = ApiResponse<object>.Fail(message, statusCode);
        httpContext.Response.Clear();
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var json = JsonSerializer.Serialize(errorResponse);
        await httpContext.Response.WriteAsync(json);
    }
}