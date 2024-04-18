using api.Setup;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class Createtag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tags", (
            [FromBody] CreateTagRequestDto dto,
            HttpContext context,
            [FromServices] ApiHelperFacade helpers,
            [FromServices] NpgsqlDataSource ds) =>
        {
            var user = helpers.SecurityService.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(helpers.KeyNamesService.JWT_KEY)!);

            using (var conn = ds.OpenConnection())
            {
                var insertedTag = conn.QueryFirst<IndependentHelpers.DomainModels.Tag>(
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

    private class CreateTagRequestDto
    {
        public string Name { get; }
    }
}