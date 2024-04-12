using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text.Json;
using api.Boilerplate.ReusableHelpers.GlobalModels;
using api.Boilerplate.ReusableHelpers.GlobalValues;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace api.Boilerplate.EndpointHelpers;

public static class ApiHelper
{
    public static User TriggerJwtValidationAndGetUserDetails(HttpContext context)
    {
        try
        {
            var jwt = context.Request.Headers[StringConstants.JwtConstants.Authorization][0] ??
                      throw new InvalidOperationException("Could not find token in headers!");
            var token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(Env.JWT_KEY)
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(jwt);

            return JsonSerializer.Deserialize<User>(JsonSerializer.Serialize(token), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false
            }) ?? throw new InvalidOperationException("Could not deserialize user from claims");
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

    
    public static void ValidateModel<T>(T model)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        Validator.ValidateObject(model, context, true);
    }
    
 
}