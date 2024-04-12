using api.Boilerplate.EndpointHelpers;
using api.Boilerplate.ReusableHelpers.GlobalModels;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class CreateTagRequestDto
{
    public string Name { get; set; }
}

public class Createtag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tags", (
            [FromBody] CreateTagRequestDto dto,
            HttpContext context,
            [FromServices] NpgsqlDataSource ds) =>
        {
            User user = ApiHelper.TriggerJwtValidationAndGetUserDetails(context);
            using (var conn = ds.OpenConnection())
            {
                var insertedTag = conn.QueryFirst<Boilerplate.ReusableHelpers.GlobalModels.Tag>(
                    "insert into todo_manager.tag (name, userid) values (@name, @userid) returning *;",
                    new
                    {
                        name = dto.Name,
                        userid = user.Id
                    }) ?? throw new InvalidOperationException("Could not create tag");
               
                return insertedTag;
            }
        });
    }
}