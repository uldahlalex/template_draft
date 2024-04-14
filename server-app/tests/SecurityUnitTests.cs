using api.Independent.GlobalValues;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using NUnit.Framework;

namespace tests;

public class SecurityUnitTests
{
    [Test]
    public void GoodJwtShouldBeVerified() => Assert.DoesNotThrow(() => JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA512Algorithm())
            .WithSecret(HardcodedValues.JWT_KEY)
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>(TestSetup.JwtForTestUser));
    

    [Test]
    public void BadJwtShouldBeDenied() => Assert.Throws<SignatureVerificationException>(() => JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA512Algorithm())
            .WithSecret(HardcodedValues.JWT_KEY)
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJVc2VybmFtZSI6ImJsYWFhaCIsIklkIjoxfQ.Bv7FjgrW7sUP4cwP0iC0Mivg207vFJj0-l-MnQxiar-C-hPVE441HKEiYZp2GhWi0XJujAWOC1q6KmNqPHKCrA"));
    
}