using EndpointHelpers.Security;

public class UtilitiesFacade(TokenService tokenService, CredentialService credentialService)
{
    public TokenService TokenService { get; } = tokenService;
    public CredentialService CredentialService { get; } = credentialService;
}