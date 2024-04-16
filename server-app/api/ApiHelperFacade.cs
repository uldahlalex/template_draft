using ApiHelpers.ApiHelpers;
using IndependentHelpers;

namespace api;

public class ApiHelperFacade(TokenService tokenService, CredentialService credentialService, EndpointHelpers endpointUtilities)
{
    public TokenService TokenService { get; } = tokenService;
    public CredentialService CredentialService { get; } = credentialService;
    public EndpointHelpers EndpointUtilities { get; } = endpointUtilities;
    
}

public static class EndpointDependentHelpersFacadeExtensions
{
    public static IServiceCollection AddDependentHelpersFacade(this IServiceCollection services)
    {
        services.AddSingleton<TokenService>();
        services.AddSingleton< CredentialService>(); 
        services.AddSingleton<ApiHelperFacade>();
        services.AddSingleton<EndpointHelpers>();
        return services;
    }
}