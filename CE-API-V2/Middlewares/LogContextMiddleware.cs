using CE_API_V2.Services.Interfaces;
using Serilog;
using Serilog.Context;

public class LogContextMiddleware
{
    private readonly RequestDelegate _next;

    public LogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserInformationExtractor userInfoExtractor)
    {
        var user = userInfoExtractor.GetUserIdInformation(context.User);
        var userId = user.UserId ?? "Anonymous";

        using (LogContext.PushProperty("aadUserName", userId))
        {
            Log.Information($"User: {userId} | Method: {context.Request.Method} | Host: {context.Request.Headers.Host} | UserAgent: {context.Request.Headers.UserAgent}");
            await _next(context);
        }
    }
}