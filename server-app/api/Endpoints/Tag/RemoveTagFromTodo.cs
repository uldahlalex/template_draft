using Carter;
using Dapper;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class RemoveTagToTodo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/tags/{tagId}/removeFromTodo/{todoId}",
            (HttpContext context,
                [FromServices] NpgsqlDataSource dataSource,
                [FromServices] ApiHelperFacade helpers,
                [FromRoute] int tagId,
                [FromRoute] int todoId) =>
            {
                var user = helpers.Security.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!);

                var sql = @"
DELETE FROM todo_manager.todo_tag
WHERE todoid = @todoId AND tagId = @tagId;
";
                using (var conn = dataSource.OpenConnection())
                {
                    var execution = conn.Execute(sql, new { todoId, tagId });
                    if (execution == 0)
                        throw new InvalidOperationException("Could not delete tag to todo.");
                }


                return new { success = true };
            });
    }
}