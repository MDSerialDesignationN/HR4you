using Microsoft.Extensions.Configuration;

namespace HR4you.Security.Authentication;

public class AuthenticationConfig
{
    public string ConnectionString { get; set; } = null!;
    public DefaultUsersAndGroups DomainUserConfig { get; set; } = new();
    public string JwtKey { get; set; }
    public string JwtIssuer { get; set; }
    public IConfigurationBuilder ConfigBuilder { get; set; }
}