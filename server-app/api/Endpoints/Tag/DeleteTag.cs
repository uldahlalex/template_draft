using Carter;
using Dapper;
using IndependentHelpers.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class DeleteTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/tag/{id}", (
            [FromRoute]int id, 
            [FromServices] EndpointHelperFacade helpers,
            HttpContext context, NpgsqlDataSource ds) =>
        {
            var user = helpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!);
            using var conn = ds.OpenConnection();
            var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
            if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            conn.Close();
        });
    }
}