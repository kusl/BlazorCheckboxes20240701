namespace BlazorCheckboxes;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Log the client IP address
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        logger.LogInformation("Client IP: {ClientIp}", clientIp);

        // Log the request headers
        foreach (var header in context.Request.Headers)
        {
            logger.LogInformation("{Header}: {Value}", header.Key, header.Value);
        }

        // Call the next middleware in the pipeline
        await next(context);
    }
}
