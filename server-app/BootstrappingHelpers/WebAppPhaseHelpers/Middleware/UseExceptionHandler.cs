using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Builder;

namespace BootstrappingHelpers.WebAppPhaseHelpers.Middleware;

public static class UseExceptionHandler
{
    public static WebApplication UseCustomExceptionHandler(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (AuthenticationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                throw;
            }
            catch (ValidationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                throw;
            }
            //todo catch for fluent validation 
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw;
            }
        });
        return app;
    }
}