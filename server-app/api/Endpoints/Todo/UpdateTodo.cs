using Carter;
using Dapper;
using IndependentHelpers.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Todo;

public class UpdateTodo : ICarterModule
{
    private class UpdateTodoRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/todos/{id}", (UpdateTodoRequestDto req,
            [FromServices] ApiHelperFacade apiHelpers,
            [FromServices] IndependentHelpersFacade indep,
            NpgsqlDataSource ds, HttpContext context) =>
        {
            var user = apiHelpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!);

            var conn = ds.OpenConnection();
            var userId = 1;
            var todo = conn.QueryFirstOrDefault<TodoWithTags>(@"
UPDATE todo_manager.todo
SET title = @Title, iscompleted = @iscompleted, description = @Description, duedate = @DueDate, priority = @Priority
WHERE id = @Id AND userid = @UserId
RETURNING *;
", req);
            conn.Close();
            return todo;
        });
    }
}