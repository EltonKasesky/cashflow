using CashFlow.Communication.Error.Responses;
using CashFlow.Exception.Exceptions;
using CashFlow.Exception.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
            HandleProjectException(context);
        else
            ThrowUnknownError(context);
    }

    private void HandleProjectException(ExceptionContext context)
    {
        CashFlowException exception = (CashFlowException)context.Exception;
        ResponseError response = new ResponseError(exception.GetErrors());

        context.HttpContext.Response.StatusCode = exception.StatusCode;
        context.Result = new ObjectResult(response);
    }

    private void ThrowUnknownError(ExceptionContext context)
    {
        ResponseError response = new ResponseError(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(response);
    }
}
