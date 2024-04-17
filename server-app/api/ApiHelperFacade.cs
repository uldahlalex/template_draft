using ApiHelperServics;
using IndependentHelpers.InjectableServices;

namespace api;

public class ApiHelperFacade(
    ValuesService valuesService,
    KeyNamesService keyNamesService,
    SecurityService securityService)
{
    public SecurityService SecurityService { get; } = securityService;
    public ValuesService ValuesService { get; } = valuesService;
    public KeyNamesService KeyNamesService { get; } = keyNamesService;
    
}

