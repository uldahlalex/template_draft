using System.Net;
using System.Security.Authentication;
using Carter;
using FluentValidation;

namespace api;

public static class MiddlewareExtensions
{
    public static WebApplication AddMiddleware(this WebApplication app)
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
            catch (System.ComponentModel.DataAnnotations.ValidationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                throw;
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                throw;
            }
        });


        app.UseStatusCodePages(async statusCodeContext =>
            await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                .ExecuteAsync(statusCodeContext.HttpContext));

        app.UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); })
            .UseCors(options =>
            {
                options.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        app.MapCarter();
        return app;
    }
}