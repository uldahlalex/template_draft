using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;

namespace api.Endpoints.UserEndpoints;

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register",
            ([FromBody] RegisterDto req, [FromServices] NpgsqlDataSource ds,
                [FromServices] AwesomeServices services
            ) =>
            {
                var salt = services.Security.GenerateSalt();
                var hash = services.Security.Hash(req.Password, salt);
                using var conn = ds.OpenConnection();
                var user = conn.QueryFirstOrDefault<User>(
                    "insert into todo_manager.user (username, passwordhash, salt) values (@Username, @PasswordHash, @Salt) RETURNING *;",
                    new
                    {
                        req.Username,
                        PasswordHash = hash,
                        Salt = salt
                    }) ?? throw new InvalidOperationException("Could not create user");
                conn.Close();

                return new AuthenticationResponseDto
                {
                    token = services.Security.IssueJwt([
                        new KeyValuePair<string, object>(nameof(user.Username), user.Username),
                        new KeyValuePair<string, object>(nameof(user.Id), user.Id)
                    ], Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!)
                };
            });
    }

    private class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}