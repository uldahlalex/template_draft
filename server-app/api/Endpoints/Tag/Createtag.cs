using Agnostics;
using api.Independent.KeysAndValues;
using Carter;
using Dapper;
using EndpointHelpers.EndpointHelpers;
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
            var user = context.VerifyJwtReturnPayloadAsT<User>(
                Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT));
            using (var conn = ds.OpenConnection())
            {
                var insertedTag = conn.QueryFirst<Agnostics.Tag>(
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