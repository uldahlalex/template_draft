using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using api.DependentHelpers.ApiHelpers;
using api.Globals.Domain;
using api.Independent;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Todo;

public class Create : ICarterModule
{
    private class CreateTodoRequestDto
    {
        [NotNull] [MinLength(1)] public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public List<Globals.Domain.Tag> Tags { get; set; } = default!;
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/todos", (
            [FromBody] CreateTodoRequestDto req,
            ApiHelperFacade apiHelpers,
            IndependentHelpers indep,
            [FromServices] NpgsqlDataSource ds,
            HttpContext context) =>
        {
            var user = apiHelpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!);
            apiHelpers.EndpointUtilities.ValidateModel(req);
            var transaction = ds.OpenConnection().BeginTransaction();
            var todo = transaction.Connection!.QueryFirstOrDefault<TodoWithTags>(@"
insert into todo_manager.todo (title, description, duedate, userid, priority)
VALUES (@Title, @Description, @DueDate, @UserId, @Priority) returning *;
        ", new
                {
                    req.Title,
                    req.Description,
                    req.DueDate,
                    UserId = user.Id,
                    req.Priority
                }
            ) ?? throw new InvalidOperationException("Could not insert todo");
            req.Tags.ForEach(e =>
            {
                if (transaction.Connection!.Execute(
                        "insert into todo_manager.todo_tag (todoid, tagid) values (@TodoId, @TagId);",
                        new { TodoId = todo.Id, TagId = e.Id }) == 0)
                    throw new InvalidOperationException("Could not associate tag with todo");
            });
            todo.Tags = transaction.Connection!.Query<Globals.Domain.Tag>(
                "select * from todo_manager.tag join todo_manager.todo_tag tt on tag.id = tt.tagid where tt.todoid = @id;",
                new { id = todo.Id }).ToList() ?? throw new InvalidOperationException("Could not retrieve tags");

            transaction.Commit();
            transaction.Connection!.Close();
            return todo;
        });
    }
}

