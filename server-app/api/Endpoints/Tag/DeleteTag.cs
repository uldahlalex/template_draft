using api.Setup;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;

namespace api.Endpoints.Tag;

public class DeleteTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/tag/{id}", (
            [FromRoute] int id,
            [FromServices] AwesomeServices services,
            HttpContext context, NpgsqlDataSource ds) =>
        {
            services.Security.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!);
            using var conn = ds.OpenConnection();
            var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
            if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            conn.Close();
        });
    }
}