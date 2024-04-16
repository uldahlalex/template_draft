using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;

namespace api.DependentHelpers.ApiHelpers;

public class EndpointHelpers
{
    public T VerifyJwtReturnPayloadAsT<T>(HttpContext context, string secret)
    {
        try
        {
            var jwt = context.Request.Headers.Authorization[0] ??
                      throw new InvalidOperationException("Could not find token in headers!");
            var token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(jwt);

            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(token), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false
            }) ?? throw new InvalidOperationException("Could not deserialize to " + typeof(T).Name);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to verify jwt and set payload as http item");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
            throw new AuthenticationException("Authentication error regarding token");
        }
    }
    public void ValidateModel<T>(T model)
    {
        //todo fluent or annotations?
        var context = new ValidationContext(model, null, null);
        Validator.ValidateObject(model, context, true);
    }
}