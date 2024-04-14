using System.Net.NetworkInformation;
using Dapper;
using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;

namespace api.DependentHelpers.BootstrappingHelpers.DbHelpers;

public static class BuildDbContainer
{
    public static async Task StartDbInContainer(string postgresConnectionString)
    {
        try
        {
            var dockerSocketPath = GetDockerSocketPath();
            var client = new DockerClientConfiguration(new Uri(dockerSocketPath)).CreateClient();
            var imageName = "postgres:latest";
            var containerName = "postgres";

            IList<ImagesListResponse> images =
                await client.Images.ListImagesAsync(new ImagesListParameters { All = true });
            if (!images.Any(i => i.RepoTags.Contains(imageName)))
                await client.Images.CreateImageAsync(
                    new ImagesCreateParameters { FromImage = imageName }, null, new Progress<JSONMessage>());

            var existingContainer = await client.Containers.ListContainersAsync(
                new ContainersListParameters
                {
                    All = true, Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        { "name", new Dictionary<string, bool> { { containerName, true } } }
                    }
                });

            if (existingContainer.Any())
                await StartExistingContainer(existingContainer, client);
            else
                await CreateContainerFromImage(client, imageName, containerName);

            var ds = new NpgsqlDataSourceBuilder(postgresConnectionString).Build();
            TestConnection(ds);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during container operations: " + ex.Message);
            Console.WriteLine(ex.InnerException);
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static void TestConnection(NpgsqlDataSource ds, int attempts = 0)
    {
        if (attempts > 100) throw new Exception("Could not start PostgreSQL container");
        try
        {
            var conn = ds.OpenConnection();
            conn.Execute("select 'hello world'");
            conn.Close();
        }
        catch (Exception)
        {
            attempts++;
            Console.WriteLine("Waiting for connection to available");
            Task.Delay(100).Wait();
            TestConnection(ds, attempts);
        }
    }

    private static async Task StartExistingContainer(IList<ContainerListResponse> existingContainer,
        DockerClient client)
    {
        var containerId = existingContainer[0].ID;
        Console.WriteLine("Existing PostgreSQL container found.");

        var isPortInUse = IsPortInUse(5432);

        if (isPortInUse)
        {
            Console.WriteLine("Port 5432 is already in use. Stopping existing container.");
            await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
        }
        else
        {
            Console.WriteLine("Port 5432 is available. Container will be reused.");
            await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
            Console.WriteLine("PostgreSQL container started.");
            return;
        }

        await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
    }

    private static async Task CreateContainerFromImage(DockerClient client, string imageName, string containerName)
    {
        var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = imageName,
            Name = containerName,
            ExposedPorts = new Dictionary<string, EmptyStruct>
            {
                { "5432/tcp", new EmptyStruct() }
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "5432/tcp", new List<PortBinding> { new() { HostPort = "5432" } } }
                }
            },
            Env = new List<string>
            {
                "POSTGRES_DB=postgres",
                "POSTGRES_USER=postgres",
                "POSTGRES_PASSWORD=postgres"
            }
        });

        await client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
    }

    private static string GetDockerSocketPath()
    {
        if (OperatingSystem.IsWindows()) return "npipe://./pipe/docker_engine";
        if (OperatingSystem.IsLinux())
        {
            //if running on github actions:
            if(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true") return "unix:///var/run/docker.sock";
                         return "unix:///home/alex/.docker/desktop/docker.sock"; //"unix:///var/run/docker.sock"; todo fix the linux path for the average user and not just me lol

        }
        if (OperatingSystem.IsMacOS()) return "unix:///var/run/docker.sock";
        throw new PlatformNotSupportedException("Unsupported operating system");
    }

    private static bool IsPortInUse(int port)
    {
        var properties = IPGlobalProperties.GetIPGlobalProperties();
        var endpoints = properties.GetActiveTcpListeners();
        return endpoints.Any(endpoint => endpoint.Port == port);
    }
}