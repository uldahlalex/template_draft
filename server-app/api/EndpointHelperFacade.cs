using ApiHelpers.ApiHelpers;

namespace api;

public class EndpointHelperFacade(
    HardcodedValues hardcodedValues,
    KeyNames keyNames,
    TokenService tokenService, 
    CredentialService credentialService, 
    EndpointHelpers endpointUtilities)
{
    public TokenService TokenService { get; } = tokenService;
    public CredentialService CredentialService { get; } = credentialService;
    public EndpointHelpers EndpointUtilities { get; } = endpointUtilities;
    public HardcodedValues HardcodedValues { get; } = hardcodedValues;
    public KeyNames KeyNames { get; } = keyNames;
    
}

// public static class EndpointDependentHelpersFacadeExtensions
// {
//     public static IServiceCollection AddDependentHelpersFacade(this IServiceCollection services)
//     {
//         services.AddSingleton<TokenService>();
//         services.AddSingleton< CredentialService>(); 
//         services.AddSingleton<ApiHelperFacade>();
//         services.AddSingleton<EndpointHelpers>();
//         return services;
//     }
// }