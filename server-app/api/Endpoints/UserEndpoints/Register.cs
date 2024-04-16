using Carter;
using Dapper;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.UserEndpoints;


public class Register : ICarterModule
{
    private class RegisterDto
 {
     public string Username { get; set; }
     public string Password { get; set; }
 }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register",
            ([FromBody] RegisterDto req, [FromServices] NpgsqlDataSource ds,
                [FromServices]ApiHelperFacade helpers) =>
            {
                var salt = helpers.Security.GenerateSalt();
                var hash = helpers.Security.Hash(req.Password, salt);
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
                    token = helpers.Security.IssueJwt([
                        new KeyValuePair<string, object>(nameof(user.Username), user.Username),
                        new KeyValuePair<string, object>(nameof(user.Id), user.Id)
                    ], Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!)
                };
            });
    }
}