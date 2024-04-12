using System.Net.Http.Headers;
using api;
using api.Boilerplate.ReusableHelpers.GlobalModels;
using api.Boilerplate.ReusableHelpers.Security;
using Microsoft.AspNetCore.Builder;

namespace tests;

public class TestSetup
{
    public WebApplication App;
    public CredentialService CredentialService = new();
    public HttpClient HttpClient = new();

    public static string JwtForTestUser =
        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJVc2VybmFtZSI6ImJsYWFhaCIsIklkIjoxfQ.1aQtDZb0Vi8tSIt5YGtgEXCtWSh_9asIMLjzFkbwrN2QOGzA4d4kMFo9MtYfTepQ2k5e5PqTGmZt46HmMxKa3A";
    
    
    public Tag TestTagHome = new Tag()
    {
        Name = "home",
        UserId = 1
    };

    public Tag TestTagWork = new Tag()
    {
        Name = "work",
        UserId = 1
    };

    
    public TodoWithTags TestTodo = new TodoWithTags
    {
        Title = "TestTodo",
        Description = "TestDescription",
        Tags = [new Tag() {Id = 1, Name = "home"}],
        DueDate = DateTime.Today,
        Id = 4,
        UserId = 1,
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