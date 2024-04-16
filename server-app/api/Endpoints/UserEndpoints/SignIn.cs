using api.DependentHelpers.ApiHelpers;
using api.Globals.Domain;
using api.Independent;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.UserEndpoints;

public class SignIn : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/signin", (
            [FromBody] AuthenticationRequestDto req,
            [FromServices] NpgsqlDataSource ds,
            [FromServices] CredentialService credService,
            [FromServices] IndependentHelpers indep,
            [FromServices] ApiHelperFacade utilitiesFacade
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

            if (utilitiesFacade.CredentialService.Hash(req.Password, userToCheck.Salt) != userToCheck.PasswordHash)
                throw new InvalidOperationException("Invalid sign-in");

            return new AuthenticationResponseDto
            {
                token = utilitiesFacade.TokenService.IssueJwt([
                    new KeyValuePair<string, object>(nameof(userToCheck.Username), userToCheck.Username),
                    new KeyValuePair<string, object>(nameof(userToCheck.Id), userToCheck.Id)
                ], Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!),
            };
        });
    }
}