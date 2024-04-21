using System.Text.Json;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;

namespace api.Endpoints.Todo;

public class GetTodosWithTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/todos", async (
            HttpContext context,
            [FromServices] NpgsqlDataSource ds,
            [FromServices] AwesomeServices services,
            [FromQuery] string serializedTagArray,
            [FromQuery] string orderBy,
            [FromQuery] string direction,
            [FromQuery] bool showCompleted,
            [FromQuery] int limit = 50) =>
        {
            var user = services.Security.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!);

            var tags = JsonSerializer.Deserialize<int[]>(serializedTagArray);
            IEnumerable<dynamic> todos;
            var filterByTags = tags == null || tags.Length == 0 ? "" : " WHERE tag.id = ANY(@Tags) ";
            var filterByNotCompleted = showCompleted ? "" : " AND t.iscompleted = false ";
            await using (var con = ds.OpenConnection())
            {
                todos = con.Query(@$"
SELECT 
    t.id, 
    t.title, 
    t.description, 
    t.duedate, 
    t.iscompleted, 
    t.createdat, 
    t.priority, 
    t.userid,
    COALESCE(json_agg(json_build_object('Id', tag.id, 'Name', tag.name)) FILTER (WHERE tag.id IS NOT NULL), '[]') AS tags
FROM todo_manager.todo t
LEFT JOIN todo_manager.todo_tag tt ON t.id = tt.todoid
LEFT JOIN todo_manager.tag tag ON tt.tagid = tag.id  
{filterByTags} 
{filterByNotCompleted}
AND t.userid = @UserId 
GROUP BY t.id
ORDER BY t.{orderBy} {direction}
LIMIT {limit};
", new
                {
                    UserId = user.Id, Tags = tags
                });
            }

//todo sql exc returned 401
            return todos.Select(row =>
            {
                var todo = new TodoWithTags
                {
                    Id = row.id,
                    Title = row.title,
                    Description = row.description,
                    DueDate = row.duedate,
                    IsCompleted = row.iscompleted,
                    CreatedAt = row.createdat,
                    Priority = row.priority,
                    UserId = row.userid,
                    Tags = JsonSerializer.Deserialize<List<IndependentHelpers.DomainModels.Tag>>(row.tags)
                };

                return todo;
            }).ToList();
        });
    }
}