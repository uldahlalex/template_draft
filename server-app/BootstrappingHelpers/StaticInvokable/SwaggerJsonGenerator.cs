using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace BootstrappingHelpers.StaticInvokable;

public static class SwaggerJsonGenerator
{
    public static void GenerateJsonFromSwaggerDefinition(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var swaggerProvider = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
            var doc = swaggerProvider.GetSwagger("v1");
            var swaggerFile = doc.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0);
            var outputPath =
                Path.Combine(serviceProvider.GetRequiredService<IWebHostEnvironment>().ContentRootPath + "/../../",
                    "swagger.json");

            File.WriteAllText(outputPath, swaggerFile);
        }
    }
}