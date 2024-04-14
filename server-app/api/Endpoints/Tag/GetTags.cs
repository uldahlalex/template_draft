using Agnostics.GlobalModels;
using Agnostics.KeysAndValues;
using api.DependentHelpers.EndpointHelpers.EndpointHelpers;
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

                List<Agnostics.GlobalModels.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<Agnostics.GlobalModels.Tag>(@"
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