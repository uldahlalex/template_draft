using System.ComponentModel.DataAnnotations;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;
using src.statics;

namespace api.Endpoints.UserEndpoints;

public class SignIn : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/signin", (
            [FromBody] SignInDto req,
            [FromServices] NpgsqlDataSource ds,
            [FromServices] AwesomeServices services
        ) =>
        {

            using var conn = ds.OpenConnection();
            var userToCheck = conn.QueryFirstOrDefault<User>(
                "SELECT * FROM todo_manager.user where username = @username;",
                new
                {
                    username = req.Username
                });
            if (userToCheck == null)
            {
                Console.WriteLine($"User {req.Username} not found in database");
                throw new InvalidOperationException("Invalid sign-in");
            }

            conn.Close();

            if (services.Security.Hash(req.Password, userToCheck.Salt) != userToCheck.PasswordHash)
                throw new InvalidOperationException("Invalid sign-in");

            return new AuthenticationResponseDto
            {
                token = services.Security.IssueJwt([
                    new KeyValuePair<string, object>(nameof(userToCheck.Username), userToCheck.Username),
                    new KeyValuePair<string, object>(nameof(userToCheck.Id), userToCheck.Id)
                ], Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!)
            };
        });
    }
public class SignInDto
{
    [MinLength(1)]
    public string Username { get; set; }
    [MinLength(4)]
    public string Password { get; set; }
}
}
