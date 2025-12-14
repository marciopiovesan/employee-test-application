
namespace Employees.Api.Middlewares
{
    public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            logger.LogInformation("Begin: {Method}-{Path}{QueryString}", context.Request.Method, context.Request.Path, context.Request.QueryString);

            await next(context);

            logger.LogInformation("End: {Method}-{Path}{QueryString}", context.Request.Method, context.Request.Path, context.Request.QueryString);
        }
    }
}
