using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BootstrappingHelpers.WebAppPhaseHelpers.Middleware;

public static class UseStatusCodePages
{
    public static WebApplication UseCustomStatusCodePages(this WebApplication app)
    {
        app.UseStatusCodePages(async statusCodeContext =>
            await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                .ExecuteAsync(statusCodeContext.HttpContext));
        return app;
    }
}