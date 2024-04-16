using ApiHelpers.ApiHelpers;
using Carter;
using Dapper;
using IndependentHelpers.Domain;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context,
[FromServicesAttribute]            EndpointHelperFacade helpers 
            ) =>
            {
                var user = helpers.EndpointUtilities.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(helpers.KeyNames.JWT_KEY)!);

                List<IndependentHelpers.Domain.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<IndependentHelpers.Domain.Tag>(@"
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