using Carter;
using Dapper;
using IndependentHelpers;
using IndependentHelpers.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using src.services;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context,
                [FromServices] AwesomeServices services
            ) =>
            {
                var user = services.Security.VerifyJwtReturnPayloadAsT<User>(context,
                    Environment.GetEnvironmentVariable(EnvVarNames.JWT_KEY)!);


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