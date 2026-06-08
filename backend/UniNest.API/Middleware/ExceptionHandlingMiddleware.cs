using System.Text.Json;

namespace UniNest.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType =
                "application/json";

            var response = new
            {
                message = ex.Message
            };

            switch (ex)
            {
                case ArgumentException:
                    context.Response.StatusCode = 400;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = 401;
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = 404;
                    break;

                default:
                    context.Response.StatusCode = 500;
                    break;
            }

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}