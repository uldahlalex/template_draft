using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace api.Boilerplate;

public class SwaggerJsonGeneratorService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(1000, stoppingToken);
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