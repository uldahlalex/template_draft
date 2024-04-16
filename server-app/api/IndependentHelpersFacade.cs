using IndependentHelpers;

namespace api;

public class IndependentHelpersFacade(KeyNames keyNames, HardcodedValues hardcodedValues, CredentialService credentialService)
{
      public KeyNames KeyNames { get; } = keyNames;
      public HardcodedValues HardcodedValues { get; } = hardcodedValues;
      public CredentialService CredentialService { get; } = credentialService;
}

public static class IndepdentHelpersFacadeExtensions
{
      public static IServiceCollection IndependentHelpers(this IServiceCollection services)
      {
            services.AddSingleton<KeyNames>();
            services.AddSingleton<HardcodedValues>();
            services.AddSingleton<CredentialService>();
            services.AddSingleton<IndependentHelpersFacade>();
            return services;
      }
}