using System.IdentityModel.Tokens.Jwt;
using HR4you.Security.Contexts;
using HR4you.Security.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4you.Security.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthenticationController: ControllerBase
{
    private readonly UserContext _db;
    private readonly JwtTokenHandler _jwtTokenHandler;
    private readonly JwtBlacklistHandler _blackList;
    private readonly PasswordHandler _passwordHandler;

    public AuthenticationController(JwtBlacklistHandler blackList, UserContext db, JwtTokenHandler jwtTokenHandler, PasswordHandler passwordHandler)
    {
        _blackList = blackList;
        _db = db;
        _jwtTokenHandler = jwtTokenHandler;
        _passwordHandler = passwordHandler;
    }
    
    [HttpPost("log-in-user")]
    [AllowAnonymous]
    [SwaggerOperation("LogUserIn")]
    public IActionResult LogUserIn (string username, [FromBody]string password) {

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("user name / password must not be empty");
        }

        var user = _db.GetUserByName(username);

        // Don't expose to the outside what is wrong with the user data
        if (user == null)
        {
            return BadRequest("login-failed"); // user does not exist
        }
        
        if (user.IsDeleted)
        {
            return BadRequest("login-failed"); // user deleted
        }

        if (!_passwordHandler.IsPasswordOk(password, user.PasswordHash!))
        {
            return BadRequest("login-failed"); // wrong password
        }

        var token = _jwtTokenHandler.GenerateJsonWebToken(user);

        user.AuthToken = token;

        _db.SaveChanges();
        return Ok(UserInfo.FromUser(user, false));
    }
    
    [HttpPost("log-out-user")]
    [SwaggerOperation("LogUserOut")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public IActionResult LogUserOut()
    {
        var userName = HttpContext.User.Identity!.Name!;
        var user = _db.GetUserByName(userName)!;
        user.AuthToken = null;
        _db.SaveChanges();
        
        return Ok();
    }
    
    [HttpGet("extent-user-login")]
    [SwaggerOperation("ExtendCurrentLogin")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    public IActionResult ExtendCurrentLogin()
    {
        var userName = HttpContext.User.Identity!.Name!;
        var user = _db.GetUserByName(userName);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var token = _jwtTokenHandler.GenerateJsonWebToken(user);
        user.AuthToken = token;
        _db.SaveChanges();
        
        return Ok(UserInfo.FromUser(user, false));
    }
    
    [HttpGet("get-token-expiry")]
    [Authorize(Policy = BuildInUserRoles.Authenticated)]
    [SwaggerOperation("GetTokenEndOfLife")]
    public IActionResult GetTokenEndOfLife()
    {
        var authorizationHeader = HttpContext.Request.Headers.Authorization;

        var token =  authorizationHeader == StringValues.Empty
            ? string.Empty
            : authorizationHeader.Single().Split(" ").Last();
        
        var handler = new JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);
        return Ok(decoded.ValidTo);
    }
    
    [HttpPost("revoke-user-token")]
    [SwaggerOperation("RevokeUserToken")]
    [Authorize(Policy = BuildInUserRoles.AdminRole)]
    public IActionResult RevokeUserToken(string userNameToRevoke)
    {
        var userToRevoke = _db.GetUserByName(userNameToRevoke);
        if (userToRevoke == null)
        {
            return NotFound("User not found");
        }
        if (userToRevoke.AuthToken == null)
        {
            return BadRequest("user has no token (not logged in)");
        }

        _blackList.Add(userToRevoke.AuthToken);
        userToRevoke.AuthToken = null;
        _db.SaveChanges();
        return Ok();
    }
}
