using api.Boilerplate.ReusableHelpers.GlobalModels;
using api.Boilerplate.ReusableHelpers.Security;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.UserEndpoints;

public class AuthenticationRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register",
            ([FromBody] AuthenticationRequestDto req, [FromServices] NpgsqlDataSource ds,
                 [FromServices] TokenService tokenservice) =>
            {
                var salt = CredentialService.GenerateSalt();
                var hash = CredentialService.Hash(req.Password, salt);
                using var conn = ds.OpenConnection();
                var user = conn.QueryFirstOrDefault<Boilerplate.ReusableHelpers.GlobalModels.User>(
                    "insert into todo_manager.user (username, passwordhash, salt) values (@Username, @PasswordHash, @Salt) RETURNING *;",
                    new
                    {
                        req.Username,
                        PasswordHash = hash,
                        Salt = salt
                    }) ?? throw new InvalidOperationException("Could not create user");
                conn.Close();

                return new AuthenticationResponseDto()
                {
                    token = TokenService.IssueJwt([
                        new KeyValuePair<string, object>(nameof(user.Username), user.Username), 
                        new KeyValuePair<string, object>(nameof(user.Id), user.Id)])
                };
            });
    }
}