using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;

namespace ApiHelperServics;

public class SecurityService
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
    public string IssueJwt(IEnumerable<KeyValuePair<string, object>> claims, string privateKey)
    {
        try
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .AddClaims(claims)
                .WithSecret(privateKey)
                .Encode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
            throw new InvalidOperationException("User authentication succeeded, but could not create token");
        }
    }
    
    public string GenerateSalt()
    {
        var bytes = new byte[128 / 8];
        using var keyGenerator = RandomNumberGenerator.Create();
        keyGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public string Hash(string password, string salt)
    {
        try
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Failed to hash password");
        }
    }
}