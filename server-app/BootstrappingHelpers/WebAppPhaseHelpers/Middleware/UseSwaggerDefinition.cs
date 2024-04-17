using Microsoft.AspNetCore.Builder;

namespace BootstrappingHelpers.WebAppPhaseHelpers.Middleware;

public static class UseSwaggerDefinition
{
    public static WebApplication UseCustomSwaggerDefinition(this WebApplication app)
    {
        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });

        return app;
    }
}