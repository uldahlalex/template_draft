using Carter;
using Dapper;
using IndependentHelpers;
using IndependentHelpers.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;

namespace api.Endpoints.Todo;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/todo/{id}", (int id,
            HttpContext context,
            [FromServices] AwesomeServices services,
            NpgsqlDataSource ds) =>
        {
            services.Security.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!);


            using (var conn = ds.OpenConnection())
            {
                var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
                if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            }

            return new { success = true };
        });
    }
}