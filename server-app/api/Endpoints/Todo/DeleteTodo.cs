using Carter;
using Dapper;
using IndependentHelpers.Domain;
using Npgsql;

namespace api.Endpoints.Todo;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/todo/{id}", (int id, HttpContext context,
            EndpointHelperFacade helpers, 
            NpgsqlDataSource ds) =>
        {
            var user = helpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!);

            using (var conn = ds.OpenConnection())
            {
                var impactedRows = conn.Execute("delete from todo_manager.todo where id = @id", new { id });
                if (impactedRows == 0) throw new InvalidOperationException("Could not delete");
            }

            return new { success = true };
        });
    }
}