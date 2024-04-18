using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using IndependentHelpers.DomainModels;
using NUnit.Framework;

namespace tests;

[TestFixture]
public class ApiTests
{
    [SetUp]
    public void BeforeEachTest()
    {
        //_setup.App.Services.GetService<DbScripts>()!.RebuildDbSchema();
        //_setup.App.Services.GetService<DbScripts>()!.SeedDB();
        _setup.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestSetup.JwtForTestUser);
    }

    private readonly TestSetup _setup = new();

    [Test]
    public void SignInTest()
    {
        _setup.HttpClient.DefaultRequestHeaders.Authorization = null;
        var response = _setup.HttpClient.PostAsJsonAsync("/api/signin", _setup.TestUser).Result;
        response.IsSuccessStatusCode.Should().BeTrue();
        // response.Content.ReadAsStringAsync().Result.Deserialize<AuthenticationResponseDto>().token.Should()
        //     .NotBeNullOrEmpty();
    }

    [Test]
    public async Task RegisterTest()
    {
        _setup.HttpClient.DefaultRequestHeaders.Authorization = null;
        var u = new User
        {
            Id = 42,
            Username = _setup.TestUser.Username + "2",
            Password = _setup.TestUser.Password + "2"
        };
        var response = _setup.HttpClient.PostAsJsonAsync("/api/register", u).Result;
        response.IsSuccessStatusCode.Should().BeTrue();
        // response.Content.ReadAsStringAsync().Result.Deserialize<AuthenticationResponseDto>().token.Should()
        //     .NotBeNullOrEmpty();
    }


    [Test]
    public async Task CreateTodo()
    {
        var response = _setup.HttpClient.PostAsJsonAsync("/api/todos", _setup.TestTodo).Result;
        response.IsSuccessStatusCode.Should().BeTrue();
        // response.Content.ReadAsStringAsync().Result.Deserialize<TodoWithTags>().ShouldNotContainNulls();
    }
}