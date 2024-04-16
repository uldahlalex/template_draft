using api.DependentHelpers.ApiHelpers;
using api.Globals.Domain;
using api.Independent;
using Carter;
using Dapper;
using Npgsql;

namespace api.Endpoints.Tag;

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", (NpgsqlDataSource ds, HttpContext context,
            EndpointHelpers epHelpers, IndependentHelpers indep
            ) =>
            {
                var user = epHelpers.VerifyJwtReturnPayloadAsT<User>(context, Environment.GetEnvironmentVariable(indep.KeyNames.JWT_KEY)!);

                List<Globals.Domain.Tag> tags;
                using (var conn = ds.OpenConnection())
                {
                    tags = conn.Query<Globals.Domain.Tag>(@"
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