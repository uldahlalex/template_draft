using api.Setup;
using Carter;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context,
                [FromServicesAttribute] ApiHelperFacade helpers
            ) =>
            {
                var user = helpers.SecurityService.VerifyJwtReturnPayloadAsT<User>(context,
                    Environment.GetEnvironmentVariable(helpers.KeyNamesService.JWT_KEY)!);

                List<IndependentHelpers.DomainModels.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<IndependentHelpers.DomainModels.Tag>(@"
select * from todo_manager.tag where userid = 1;
")
                        .ToList();
                    conn.Close();
                }

                return tags;
            }
        );
    }
}