using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace api.Boilerplate;

public static class ExceptionHandler
{
    public static WebApplication UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;
                    int statusCode;
                    if (exception is AuthenticationException)
                    {
                        statusCode = StatusCodes.Status401Unauthorized;
                    }
                    else if (exception is ValidationException ||
                             exception is System.ComponentModel.DataAnnotations.ValidationException)
                    {
                        statusCode = StatusCodes.Status400BadRequest;
                    }
                    else
                    {
                        statusCode = StatusCodes.Status500InternalServerError;
                    }

                    if (!context.Response.HasStarted)
                    {
                        context.Response.StatusCode = statusCode;
                    }
                }
            });
        });
        return app;
    }
}