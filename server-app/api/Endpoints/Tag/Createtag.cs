using Carter;
using Dapper;
using IndependentHelpers.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class Createtag : ICarterModule
{
    private class CreateTagRequestDto
    {
        public string Name { get; set; }
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tags", (
            [FromBody] CreateTagRequestDto dto,
            HttpContext context,
            [FromServices] IndependentHelpersFacade indep,
            [FromServices] ApiHelperFacade epHelpers,
            [FromServices] NpgsqlDataSource ds) =>
        {
            var user = epHelpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context,
                Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!);

            using (var conn = ds.OpenConnection())
            {
                var insertedTag = conn.QueryFirst<IndependentHelpers.Domain.Tag>(
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