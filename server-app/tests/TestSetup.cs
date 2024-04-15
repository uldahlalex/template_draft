using Agnostics.GlobalModels;
using api;
using api.DependentHelpers.EndpointHelpers.Security;
using Microsoft.AspNetCore.Builder;

namespace tests;

public class TestSetup
{
    public static string JwtForTestUser =
        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJVc2VybmFtZSI6ImJsYWFhaCIsIklkIjoxfQ.1aQtDZb0Vi8tSIt5YGtgEXCtWSh_9asIMLjzFkbwrN2QOGzA4d4kMFo9MtYfTepQ2k5e5PqTGmZt46HmMxKa3A";

    public WebApplication App;
    public CredentialService CredentialService = new();
    public HttpClient HttpClient = new();


    public Tag TestTagHome = new()
    {
        Name = "home",
        UserId = 1
    };

    public Tag TestTagWork = new()
    {
        Name = "work",
        UserId = 1
    };


    public TodoWithTags TestTodo = new()
    {
        Title = "TestTodo",
        Description = "TestDescription",
        Tags = [new Tag { Id = 1, Name = "home" }],
        DueDate = DateTime.Today,
        Id = 4,
        UserId = 1
    };


    public User TestUser = new()
    {
        Id = 1,
        Username = "blaaah",
        Password = "blaaah"
    };

    public TokenService TokenService = new();

    public TestSetup()
    {
        HttpClient.BaseAddress = new Uri("http://localhost:9999");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        App = Program.Startup().Result;
        App.StartAsync();
    }
}