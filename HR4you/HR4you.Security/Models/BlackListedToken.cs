using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;

namespace HR4you.Security.Models;

[Table("hr4you_blacklistedToken")]
public class BlacklistedToken
{
    public BlacklistedToken(string token)
    {
        Token = token;
        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);
        Expire = decoded.ValidTo;
    }

    public long Id { get; set; }
    public string Token { get; init; }
    public DateTime Expire { get; set; }
}