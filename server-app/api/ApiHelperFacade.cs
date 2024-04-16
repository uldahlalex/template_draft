using ApiHelpers.ApiHelpers;

namespace api;

public class ApiHelperFacade(
    Values values,
    KeyNames keyNames,
    Security security)
{
    public Security Security { get; } = security;
    public Values Values { get; } = values;
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