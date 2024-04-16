using EndpointHelpers.Security;

public class UtilitiesFacade(TokenService tokenService, CredentialService credentialService)
{
    public TokenService TokenService { get; } = tokenService;
    public CredentialService CredentialService { get; } = credentialService;
}

public static class UtilitiesFacadeExtensions
{
    public static IServiceCollection AddUtilitiesFacade(this IServiceCollection services)
    {
        services.AddSingleton<TokenService>();
        services.AddSingleton<UtilitiesFacade>();
        services.AddSingleton<CredentialService>();
        return services;
    }
}