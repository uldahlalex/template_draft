using Microsoft.AspNetCore.Builder;

namespace BootstrappingHelpers.WebAppPhaseHelpers.Middleware;

public static class UseCors
{
    public static WebApplication UseCustomCors(this WebApplication app)
    {
        app.UseCors(options =>
        {
            options.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });

        return app;
    }
}