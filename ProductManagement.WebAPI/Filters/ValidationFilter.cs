using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductManagement.Core.DTOs.ApiResponses;

namespace ProductManagement.WebAPI.Filters;

/// <summary>
///     Validation filter for FluentValidation which is used to validate the request model.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            // Create the ErrorDto with validation errors
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                })
                .ToList();

            // Create a list of error messages
            var errorMessages = errorsInModelState
                .SelectMany(e => e.Errors
                    .Select(error => new
                    {
                        FieldName = e.Field,
                        Message = error
                    }))
                .ToList();

            // Create a list of errors for the ErrorDto
            var errorList = errorMessages
                .Select(e => $"{e.FieldName}: {e.Message}")
                .ToList();

            var errorDto = new ErrorDto(errorList);

            // Create the ApiResponse with the ErrorDto
            var apiResponse = ApiResponse<object>.Fail(errorDto, StatusCodes.Status400BadRequest);

            // Set the result to BadRequest with ApiResponse
            context.Result = new BadRequestObjectResult(apiResponse);
            return;
        }

        await next();
    }
}