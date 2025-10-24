using System.Net;
using System.Text.Json;

namespace Carsales.Bff.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error calling upstream service");
            await WriteProblem(context, HttpStatusCode.BadGateway, "Upstream error", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, HttpStatusCode.InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(HttpContext ctx, HttpStatusCode code, string title, string detail)
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)code;

        var payload = new { title, status = (int)code, detail, traceId = ctx.TraceIdentifier };
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
