using System.Text;
using CE_API_V2.Constants;

namespace CE_API_V2.Controllers.Middlewares;

public class StaticInformationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public StaticInformationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("x-api-version", ApiVersion.CommitId);
        context.Response.Headers.Add("ce-logo", "/images/CELogo.png");
        context.Response.Headers.Add("ivd-logo", "/images/IVDLogo.png");
        context.Response.Headers.Add("EcRepContactInfo", "/images/NemiusBanner.png");
        context.Response.Headers.Add("ifu-url", _configuration.GetValue<string>("ifu-url"));
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}