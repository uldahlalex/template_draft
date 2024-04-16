using Agnostics;
using api.Independent.KeysAndValues;
using Carter;
using Dapper;
using EndpointHelpers.EndpointHelpers;
using Npgsql;

namespace api.Endpoints.Tag;

public class DeleteTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/tag/{id}", (int id, HttpContext context, NpgsqlDataSource ds) =>
        {
            context.VerifyJwtReturnPayloadAsT<User>(Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)!);
            using var conn = ds.OpenConnection();
            var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
            if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            conn.Close();
        });
    }
}