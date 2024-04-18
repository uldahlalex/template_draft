using api.Setup;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.UserEndpoints;

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register",
            ([FromBody] RegisterDto req, [FromServices] NpgsqlDataSource ds,
                [FromServices] ApiHelperFacade helpers) =>
            {
                var salt = helpers.SecurityService.GenerateSalt();
                var hash = helpers.SecurityService.Hash(req.Password, salt);
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
                    token = helpers.SecurityService.IssueJwt([
                        new KeyValuePair<string, object>(nameof(user.Username), user.Username),
                        new KeyValuePair<string, object>(nameof(user.Id), user.Id)
                    ], Environment.GetEnvironmentVariable(helpers.KeyNamesService.JWT_KEY)!)
                };
            });
    }

    private class RegisterDto
    {
        public string Username { get; }
        public string Password { get; }
    }
}