using Employees.Application.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Middlewares
{
    public class GlobalExceptionHandler(ILogger logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, $"An unhandled exception occurred while processing the request {httpContext.Request.Path}");

            var result = Result.Failure(ErrorTypes.Internal, exception.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
            
            return true;
        }
    }
}
