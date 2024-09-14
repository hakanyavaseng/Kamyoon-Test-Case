using Microsoft.AspNetCore.Mvc.Filters;
using ProductManagement.Core.Interfaces.UnitOfWork;

namespace ProductManagement.WebAPI.Filters;

public class TransactionFilter : IAsyncActionFilter
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionFilter(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(default);

        var resultContext = await next();

        if (resultContext.Exception != null)
        {
            await transaction.RollbackAsync();
        }
        else
        {
            await _unitOfWork.SaveChangesAsync(default);
            await transaction.CommitAsync();
        }
    }
}