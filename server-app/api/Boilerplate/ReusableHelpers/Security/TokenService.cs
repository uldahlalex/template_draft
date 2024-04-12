using api.Boilerplate.ReusableHelpers.GlobalModels;
using api.Boilerplate.ReusableHelpers.GlobalValues;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;

namespace api.Boilerplate.ReusableHelpers.Security;

public class TokenService
{
    public static string IssueJwt(IEnumerable<KeyValuePair<string,object>> claims)
    {
        try
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA512Algorithm())  
                .AddClaims(claims)
                .WithSecret(Env.JWT_KEY)
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