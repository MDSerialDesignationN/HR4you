using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HR4you.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HR4you.Security.Handler;

public class JwtTokenHandler
{
    private readonly IConfiguration _config;

    public JwtTokenHandler(IConfiguration config)
    {
        _config = config;
    }
    
    public string GenerateJsonWebToken(User user)    
    {    
        var userRoles = user.GetRoles();
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        roleClaims.Add(new Claim(ClaimTypes.Role, BuildInUserRoles.Authenticated));
        
        var userClaims =new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };
        
        userClaims.AddRange(roleClaims);
        var validInMin = _config.GetValue<int>("Jwt:ValidInMin");
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
            _config["Jwt:Issuer"],    
            userClaims,    
            expires: DateTime.Now.AddMinutes(validInMin),    
            signingCredentials: credentials);    
    
        return new JwtSecurityTokenHandler().WriteToken(token);    
    }  
    
}