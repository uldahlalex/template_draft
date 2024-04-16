using JWT.Algorithms;
using JWT.Builder;

namespace ApiHelpers.ApiHelpers;


public class TokenService 
{
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
}
