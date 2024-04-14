using Agnostics.GlobalModels;
using Agnostics.KeysAndValues;
using api.DependentHelpers.EndpointHelpers.EndpointHelpers;
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
            User user = HttpContextExtensions.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(KeyNames.ASPNETCORE_ENVIRONMENT));
            using (var conn = ds.OpenConnection())
            {
                var insertedTag = conn.QueryFirst<Agnostics.GlobalModels.Tag>(
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