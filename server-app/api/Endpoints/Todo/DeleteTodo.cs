
using api.DependentHelpers.ApiHelpers;
using api.Globals.Domain;
using api.Independent;
using Carter;
using Dapper;
using Npgsql;

namespace api.Endpoints.Todo;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/todo/{id}", (int id, HttpContext context,
            ApiHelperFacade apiHelpers, 
            IndependentHelpers indep,
            NpgsqlDataSource ds) =>
        {
            var user = apiHelpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!);

            using (var conn = ds.OpenConnection())
            {
                var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
                if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            }

            return new { success = true };
        });
    }
}