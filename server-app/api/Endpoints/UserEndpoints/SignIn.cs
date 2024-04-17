using Carter;
using Dapper;
using IndependentHelpers.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.UserEndpoints;

public class SignIn : ICarterModule
{
    private class SignInDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/signin", (
            [FromBody] SignInDto req,
            [FromServices] NpgsqlDataSource ds,
            [FromServices] ApiHelperFacade helpers
            ) =>
        {
            using var conn = ds.OpenConnection();
            var userToCheck = conn.QueryFirstOrDefault<User>(
                "SELECT * FROM todo_manager.user where username = @username;",
                new
                {
                    req.Username
                }) ?? throw new InvalidOperationException("Invalid sign-in");
            conn.Close();

            if (helpers.SecurityService.Hash(req.Password, userToCheck.Salt) != userToCheck.PasswordHash)
                throw new InvalidOperationException("Invalid sign-in");

            return new AuthenticationResponseDto
            {
                token = helpers.SecurityService.IssueJwt([
                    new KeyValuePair<string, object>(nameof(userToCheck.Username), userToCheck.Username),
                    new KeyValuePair<string, object>(nameof(userToCheck.Id), userToCheck.Id)
                ], Environment.GetEnvironmentVariable(helpers.KeyNamesService.JWT_KEY)!),
            };
        });
    }
}