using System.Reflection;
using CE_API_V2.Constants;
namespace CE_API_V2.Controllers.Middlewares;

public class StaticInformationMiddleware
{
    private readonly RequestDelegate _next;

    public StaticInformationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        context.Response.Headers.Add("x-api-version", ApiVersion.CommitId);
        
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}