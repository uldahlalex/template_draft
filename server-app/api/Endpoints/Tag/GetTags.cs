using api.EndpointHelpers.EndpointHelpers;
using api.Independent.GlobalModels;
using api.Independent.GlobalValues;
using Carter;
using Dapper;
using Npgsql;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context) =>
            {
                HttpContextExtensions.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)!);

                List<Independent.GlobalModels.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<Independent.GlobalModels.Tag>(@"
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