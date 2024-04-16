namespace api.Independent;

public class IndependentHelpers(KeyNames keyNames, HardcodedValues hardcodedValues)
{
      public KeyNames KeyNames { get; } = keyNames;
      public HardcodedValues HardcodedValues { get; } = hardcodedValues;
}

public static class IndepdentHelpersFacadeExtensions
{
      public static IServiceCollection IndependentHelpers(this IServiceCollection services)
      {
            services.AddSingleton<KeyNames>();
            services.AddSingleton<HardcodedValues>();
            services.AddSingleton<IndependentHelpers>();
            return services;
      }
}