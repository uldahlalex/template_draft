using Agnostics;
using api.Independent.KeysAndValues;
using Carter;
using Dapper;
using EndpointHelpers.EndpointHelpers;
using Npgsql;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context) =>
            {
                context.VerifyJwtReturnPayloadAsT<User>(Environment.GetEnvironmentVariable(KeyNames.JWT_KEY)!);

                List<Agnostics.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<Agnostics.Tag>(@"
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