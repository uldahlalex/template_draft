using Carter;
using Dapper;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class AddTagToTodo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tags/{tagId}/addToTodo/{todoId}",
            (HttpContext context,
                [FromServices] NpgsqlDataSource dataSource,
                [FromServices] ApiHelperFacade helpers,
                [FromRoute] int tagId,
                [FromRoute] int todoId) =>
            {
                helpers.Security.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!);
                var sql = @"
INSERT INTO todo_manager.todo_tag (todoid, tagid)
VALUES (@todoId, @tagId);";
                using (var conn = dataSource.OpenConnection())
                {
                    var execution = conn.Execute(sql, new { todoId, tagId });
                    if (execution == 0)
                        throw new InvalidOperationException("Could not add tag to todo.");
                }
                return new { success = true };
            });
    }
}