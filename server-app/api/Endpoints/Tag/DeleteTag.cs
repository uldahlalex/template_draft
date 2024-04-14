using Agnostics.GlobalModels;
using Agnostics.KeysAndValues;
using api.DependentHelpers.EndpointHelpers.EndpointHelpers;
using Carter;
using Dapper;
using Npgsql;

namespace api.Endpoints.Tag;

public class DeleteTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/tag/{id}", (int id, HttpContext context, NpgsqlDataSource ds) =>
        {
            HttpContextExtensions.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)!);
            using var conn = ds.OpenConnection();
            var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
            if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            conn.Close();
        });
    }
}